module Render

open Browser.Types
open CameraState
open FiveNightsAtxMax_
open FiveNightsAtxMax_.RenderBasics
open Game
open RenderBasics
open Fable.Core.JS

type GL = WebGLRenderingContext

let drawStillImage url renderData program =
    let gl = renderData.gl
    let textures = renderData.textures

    let ourTex = textures.get url

    let inline setAttribLocations () =
        let aPosLocation = gl.getAttribLocation (program, "aPosition")
        gl.vertexAttribPointer (aPosLocation, 2, gl.FLOAT, false, 4. * 4., 0)
        gl.enableVertexAttribArray (aPosLocation)
        let aPosLocation = gl.getAttribLocation (program, "aTexCoord")
        gl.vertexAttribPointer (aPosLocation, 2, gl.FLOAT, false, 4. * 4., 2. * 4.)
        gl.enableVertexAttribArray (aPosLocation)

    let inline useProgram () = gl.useProgram (program)

    let inline loadBuffer () =
        let _ = createBuffer gl gl.ARRAY_BUFFER (fullScreenBuffer |> unbox)
        let _ = createBuffer gl gl.ELEMENT_ARRAY_BUFFER (quadIndexes |> unbox)
        ()

    let inline bindTex () =
        gl.activeTexture (gl.TEXTURE0)
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

let prepare renderData =
    let gl = renderData.gl
    gl.viewport (0, 0, gl.drawingBufferWidth, gl.drawingBufferHeight)


let inline renderRoom (gs: gameState) rd =
    let drawRoom location =
        let locationString =
            match location with
            | CamLeftCorridor -> "img/LeftCorridorOpen.png"
            | CamRightCorridor -> "img/RightCorridorOpen.png"
            | CamBossOffice -> "img/BossOffice.png"
            | CamEntryRoom -> "img/EntryRoom.png"
            | CamLeftHall -> "img/LeftHall.png"
            | CamRightHall -> "img/RightHall.png"
            | CamLeftBackStage -> "img/LeftBackStage.png"
            | CamRightBackStage -> "img/RightBackStage.png"
            | CamKitchen -> "img/Kitchen.png"

        drawStillImage locationString rd (programWithStatic rd)

    let drawDoor rl =
        function
        | Closed -> ()
        | Closing 0 -> ()
        | Opening 0 -> ()
        | Closing n
        | Opening n ->
            console.log n
            sprintf "img/door/%s%d.png" rl n
            |> fun d -> drawStillImage d rd (program rd)
        | _ -> ()      
    
    let drawOffice =
        drawDoor "LeftDoor" gs.leftDoor
        drawDoor "RightDoor" gs.rightDoor 
        drawStillImage "img/InOffice00.png" rd (program rd)

    match gs.pov with
    | _, monitorStatus.InOffice -> drawOffice
    | location, _ -> drawRoom location



let inline updateUniforms (gameState: gameState) (rd: renderData) =
    let gl = rd.gl
    let programWithStatic = programWithStatic rd

    let setUniform program name value =
        let timeUniLoc = gl.getUniformLocation (program, name)
        gl.uniform1f (timeUniLoc, value)

    setUniform programWithStatic "time" gameState.time


let draw gameState renderData =
    let gl = renderData.gl
    gl.clearColor (0, 0, 1, 1)
    gl.clear (float (int gl.COLOR_BUFFER_BIT ||| int gl.DEPTH_BUFFER_BIT))
    updateUniforms gameState renderData
    renderRoom gameState renderData
    ()
