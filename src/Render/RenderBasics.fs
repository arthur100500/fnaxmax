module RenderBasics

open Browser.Types
open Fable.Core.JS
open Fable.Core.JsInterop
open Shaders
open Utils
open Textures
type GL = WebGLRenderingContext


let createShader (gl: GL) sourceCode t =
    let shader = gl.createShader t
    gl.shaderSource (shader, sourceCode)
    gl.compileShader shader

    if unbox gl.getShaderParameter (shader, gl.COMPILE_STATUS) |> not then
        let info = unbox gl.getShaderInfoLog shader
        console.error("Error compiling shader", info)

    shader


let createShaderProgramNonMem ((gl: GL), vertex, fragment) =
    let vertexShader = createShader gl vertex gl.VERTEX_SHADER
    let fragShader = createShader gl fragment gl.FRAGMENT_SHADER

    let program = gl.createProgram ()
    gl.attachShader (program, vertexShader)
    gl.attachShader (program, fragShader)
    gl.linkProgram (program)
    
    if (gl.getProgramParameter(program, gl.LINK_STATUS) |> unbox |> not) then
        let info = gl.getProgramInfoLog(program);
        console.error("ERROR linking program!", info)
    
    gl.validateProgram program
    
    if (gl.getProgramParameter(program, gl.VALIDATE_STATUS) |> unbox |> not) then
        console.error("ERROR validating program!", gl.getProgramInfoLog(program))
        
    console.log "Program created successfully"
    program

type renderData =
    { gl: GL
      textures: FSharp.Collections.Map<texture, WebGLTexture> }
let createShaderProgram = memoize createShaderProgramNonMem

let createBuffer (gl:GL) t items =
    let buffer = gl.createBuffer()
    gl.bindBuffer(t, buffer)
    gl.bufferData(t, items, gl.STATIC_DRAW)

    buffer
    
let fullScreenBuffer = createNew Constructors.Float32Array
                           [| -1; -1; 0; 0; 1; -1; 1; 0; -1; 1; 0; 1; 1; 1; 1; 1; 1; -1; 1; 0; -1; 1; 0; 1 |]
let quadIndexes = createNew Constructors.Int16Array [|0; 1; 3; 0; 2; 3|]


let usualProgram (rd: renderData) = createShaderProgram (rd.gl, vertexShader, fragmentShader)
let programWithStatic (rd: renderData) = createShaderProgram (rd.gl, vertexShader, staticFragmentShaders)

