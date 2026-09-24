import Foundation
import AVFoundation
import CoreGraphics
import ImageIO
import CoreVideo

let input = CommandLine.arguments[1]
let output = CommandLine.arguments[2]
let src = CGImageSourceCreateWithURL(URL(fileURLWithPath: input) as CFURL, nil)!
let art = CGImageSourceCreateImageAtIndex(src, 0, nil)!
let width = 1280
let height = 720
let fps = 24
let seconds = 12
let writer = try AVAssetWriter(outputURL: URL(fileURLWithPath: output), fileType: .mp4)
let settings: [String: Any] = [
  AVVideoCodecKey: AVVideoCodecType.h264,
  AVVideoWidthKey: width,
  AVVideoHeightKey: height,
  AVVideoCompressionPropertiesKey: [
    AVVideoAverageBitRateKey: 2_800_000,
    AVVideoMaxKeyFrameIntervalKey: fps
  ]
]
let inputTrack = AVAssetWriterInput(mediaType: .video, outputSettings: settings)
inputTrack.expectsMediaDataInRealTime = false
let attributes: [String: Any] = [
  kCVPixelBufferPixelFormatTypeKey as String: kCVPixelFormatType_32ARGB,
  kCVPixelBufferWidthKey as String: width,
  kCVPixelBufferHeightKey as String: height,
  kCVPixelBufferCGImageCompatibilityKey as String: true,
  kCVPixelBufferCGBitmapContextCompatibilityKey as String: true
]
let adaptor = AVAssetWriterInputPixelBufferAdaptor(assetWriterInput: inputTrack, sourcePixelBufferAttributes: attributes)
writer.add(inputTrack)
writer.startWriting()
writer.startSession(atSourceTime: .zero)
if writer.status == .failed { throw writer.error ?? NSError(domain: "IntroWriter", code: 1) }

func clamp(_ v: Double, _ lo: Double = 0, _ hi: Double = 1) -> Double { min(hi, max(lo, v)) }
func smooth(_ v: Double) -> Double { let x = clamp(v); return x*x*(3-2*x) }
func blend(_ a: Double, _ b: Double, _ t: Double) -> Double { a+(b-a)*t }

for frame in 0..<(fps*seconds) {
  while !inputTrack.isReadyForMoreMediaData { Thread.sleep(forTimeInterval: 0.003) }
  let t = Double(frame)/Double(fps)
  var raw: CVPixelBuffer?
  CVPixelBufferCreate(kCFAllocatorDefault, width, height, kCVPixelFormatType_32ARGB, attributes as CFDictionary, &raw)
  let pixel = raw!
  CVPixelBufferLockBaseAddress(pixel, [])
  let ctx = CGContext(data: CVPixelBufferGetBaseAddress(pixel), width: width, height: height,
    bitsPerComponent: 8, bytesPerRow: CVPixelBufferGetBytesPerRow(pixel),
    space: CGColorSpaceCreateDeviceRGB(), bitmapInfo: CGImageAlphaInfo.noneSkipFirst.rawValue)!
  ctx.setFillColor(CGColor(gray: 0, alpha: 1))
  ctx.fill(CGRect(x: 0, y: 0, width: width, height: height))

  // A first-person head lift: begin on the pavement, then reveal the station.
  let lift = smooth((t-0.65)/3.1)
  let walking = smooth((t-5.1)/1.0) * (1-smooth((t-8.8)/0.5))
  let stepPhase = (t-5.1)*2.15*Double.pi
  let bob = walking * (5.0*sin(stepPhase) + 2.0*sin(2*stepPhase))
  let push = walking * clamp((t-5.1)/3.8)
  let scale = blend(1.65, 1.0, lift) + push*0.15
  let drawWidth = Double(width)*scale
  let drawHeight = Double(height)*scale
  let y = blend(150.0, 0.0, lift) - push*12 + bob
  ctx.interpolationQuality = .none
  ctx.draw(art, in: CGRect(x: (Double(width)-drawWidth)/2, y: y,
    width: drawWidth, height: drawHeight))

  // Eyelids part slowly rather than using an instant cut.
  let opening = smooth((t-0.55)/2.15)
  let lid = (1-opening)*Double(height)*0.52
  ctx.setFillColor(CGColor(gray: 0, alpha: 1))
  ctx.fill(CGRect(x: 0, y: 0, width: Double(width), height: lid))
  ctx.fill(CGRect(x: 0, y: Double(height)-lid, width: Double(width), height: lid))

  // Final blackout carries the player into the office.
  let fade = smooth((t-9.05)/1.35)
  if fade > 0 {
    ctx.setFillColor(CGColor(gray: 0, alpha: fade))
    ctx.fill(CGRect(x: 0, y: 0, width: width, height: height))
  }
  CVPixelBufferUnlockBaseAddress(pixel, [])
  adaptor.append(pixel, withPresentationTime: CMTime(value: CMTimeValue(frame), timescale: CMTimeScale(fps)))
}
inputTrack.markAsFinished()
let semaphore = DispatchSemaphore(value: 0)
writer.finishWriting { semaphore.signal() }
semaphore.wait()
if writer.status != .completed { throw writer.error ?? NSError(domain: "Intro", code: 1) }
print(output)
