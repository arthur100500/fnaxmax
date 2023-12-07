module RenderBasics

open Browser.Types
open Textures
open Fable.Core.JS
open Fable.Core

type GL = WebGLRenderingContext
type renderData =
    { gl: GL
      textures: FSharp.Collections.Map<texture, WebGLTexture> }

val usualProgram: renderData -> WebGLProgram
val programWithStatic: renderData -> WebGLProgram
val createBuffer: GL -> float -> U3<float,ArrayBufferView,ArrayBuffer> -> WebGLBuffer
val fullScreenBuffer: obj
val quadIndexes: obj