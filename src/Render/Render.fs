module Render

open Browser.Types
open Fable.Core.JS
open RenderBasics

type GL = WebGLRenderingContext

let inline drawStillImage tex renderData program =
    let gl = renderData.gl
    let textures = renderData.textures

    let ourTex = FSharp.Collections.Map.find tex textures

    let inline setAttribLocations () =
        let aPosLocation = gl.getAttribLocation (program, "aPosition")
        gl.vertexAttribPointer (aPosLocation, 2, gl.FLOAT, false, 4. * 4., 0)
        gl.enableVertexAttribArray aPosLocation
        let aPosLocation = gl.getAttribLocation (program, "aTexCoord")
        gl.vertexAttribPointer (aPosLocation, 2, gl.FLOAT, false, 4. * 4., 2. * 4.)
        gl.enableVertexAttribArray aPosLocation

    let inline useProgram () = gl.useProgram program

    let inline loadBuffer () =
        let _ = createBuffer gl gl.ARRAY_BUFFER (fullScreenBuffer |> unbox)
        let _ = createBuffer gl gl.ELEMENT_ARRAY_BUFFER (quadIndexes |> unbox)
        ()

    let inline bindTex () =
        gl.activeTexture gl.TEXTURE0
        gl.bindTexture (gl.TEXTURE_2D, ourTex)

    let inline drawArrays () =
        gl.drawElements (gl.TRIANGLES, 6, gl.UNSIGNED_SHORT, 0)

    let inline setUniform () =
        let imgLocation = gl.getUniformLocation (program, "img0")
        gl.uniform1i (imgLocation, 0.)

    loadBuffer ()
    useProgram ()
    setAttribLocations ()
    bindTex ()
    setUniform ()
    drawArrays ()
    
let inline prepare renderData =
    let gl = renderData.gl
    gl.viewport (0, 0, gl.drawingBufferWidth, gl.drawingBufferHeight)
    gl.enable gl.BLEND
    gl.blendFunc (gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA)

let inline updateUniforms (time: float) (rd: renderData) =
    let gl = rd.gl
    let programWithStatic = programWithStatic rd

    let setUniform program name value =
        gl.useProgram program
        let timeUniLoc = gl.getUniformLocation (program, name)
        gl.uniform1f (timeUniLoc, value)

    setUniform programWithStatic "time" time

let inline clear time renderData =
    let gl = renderData.gl
    gl.clearColor (0, 0, 1, 1)
    gl.clear (float (int gl.COLOR_BUFFER_BIT ||| int gl.DEPTH_BUFFER_BIT))
    updateUniforms time renderData
