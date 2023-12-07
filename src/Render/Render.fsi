module Render

open Browser.Types
open RenderBasics
open Textures

val inline clear: float -> renderData -> unit
val inline prepare: renderData -> unit
val inline drawStillImage: texture -> renderData -> WebGLProgram -> unit