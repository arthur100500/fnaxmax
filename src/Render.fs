module Render

open Browser.Types
open CameraState
open FiveNightsAtxMax_.RenderBasics
open Game
open Locations

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
    gl.enable gl.BLEND
    // gl.enable gl.DEPTH_TEST
    gl.blendFunc (gl.SRC_ALPHA, gl.ONE_MINUS_SRC_ALPHA)


let inline renderRoom (gs: gameState) rd =
    let drawRoom location =
        let locationString =
            match location with
            | CamLeftCorridor when gs.leftDoor = Open -> "img/LeftCorridorOpen.png"
            | CamRightCorridor when gs.rightDoor = Open -> "img/RightCorridorOpen.png"
            | CamLeftCorridor -> "img/LeftCorridorClosed.png"
            | CamRightCorridor -> "img/RightCorridorClosed.png"
            | CamBossOffice -> "img/BossOffice.png"
            | CamEntryRoom -> "img/EntryRoom.png"
            | CamLeftHall -> "img/LeftHall.png"
            | CamRightHall -> "img/RightHall.png"
            | CamLeftBackStage -> "img/LeftBackStage.png"
            | CamRightBackStage -> "img/RightBackStage.png"
            | CamKitchen -> "img/Kitchen.png"

        drawStillImage locationString rd (programWithStatic rd)
        
        let drawMax m =
            let _, loc, _ = m
            let maxImage =
                match loc, location with
                | Kitchen, CamKitchen -> Some "img/xmax/xMaxKitchen.png"
                | BossOffice, CamBossOffice -> Some "img/xmax/xMaxBossOffice.png"
                | LeftHall, CamLeftHall -> Some "img/xmax/xMaxLeftHall.png"
                | RightCorridor, CamRightCorridor -> Some "img/xmax/xMaxRightCorridor.png"
                | LeftBackStage, CamLeftBackStage -> Some "img/xmax/xMaxLeftBackStage.png"
                | LeftStage, CamLeftHall -> Some "img/xmax/xMaxLeftStage.png"
                | RightStage, CamRightHall -> Some "img/xmax/xMaxRightStage.png"
                | LeftCorridor, CamLeftCorridor -> Some "img/xmax/xMaxLeftCorridor.png"
                | RightHall, CamRightHall -> Some "img/xmax/xMaxRightHall.png"
                | _ -> None
            match maxImage with
            | Some img ->
                drawStillImage img rd (programWithStatic rd)
            | None -> ()
        
        List.iter drawMax gs.enemies

    let drawDoor rl =
        function
        | Open -> ()
        | Closing 0 -> ()
        | Opening 0 -> ()
        | Closing n
        | Opening n ->
            sprintf "img/door/%s%d.png" rl n
            |> fun d -> drawStillImage d rd (program rd)
        | Closed ->
            sprintf "img/door/%s3.png" rl
            |> fun d -> drawStillImage d rd (program rd)   
    
    let drawOffice =
        drawStillImage "img/Office.png" rd (program rd)
        drawDoor "LeftDoor" gs.leftDoor
        drawDoor "RightDoor" gs.rightDoor 

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
