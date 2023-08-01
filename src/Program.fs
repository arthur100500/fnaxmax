module Main

open FiveNightsAtxMax_
open Game
open Textures
open Render
open RenderBasics
open Browser
open Browser.Types

let inline bindButtons () =
    let lightRight = document.getElementById "light-right"
    let lightLeft = document.getElementById "light-left"
    let camSwitch = document.getElementById "camera-up"
    let camRight = document.getElementById "camera-right"
    let camLeft = document.getElementById "camera-left"
    lightRight.onclick <- fun _ -> globalControls.lightRight <- not globalControls.lightRight
    lightLeft.onclick <- fun _ -> globalControls.lightLeft <- not globalControls.lightLeft
    camSwitch.onclick <- fun _ -> globalControls.cameraSwitch <- true
    camRight.onclick <- fun _ -> globalControls.cameraRight <- true
    camLeft.onclick <- fun _ -> globalControls.cameraLeft <- true
    
    
    
let gl: WebGLRenderingContext =
    let canvas: Types.HTMLCanvasElement = unbox document.getElementById "game-canvas"
    unbox canvas.getContext "webgl"

let textures =
    [ "img/RightCorridorClosed.png"
      "img/LeftCorridorClosed.png"
      "img/RightCorridorOpen.png"
      "img/LeftCorridorOpen.png"
      "img/RightBackStage.png"
      "img/LeftBackStage.png"
      "img/RightLight.png"
      "img/BossOffice.png"
      "img/RightHall.png"
      "img/LeftLight.png"
      "img/EntryRoom.png"
      "img/LeftHall.png"
      "img/Kitchen.png"
      "img/Office.png"
      "img/door/RightDoor1.png"
      "img/door/RightDoor2.png"
      "img/door/RightDoor3.png"
      "img/door/LeftDoor1.png"
      "img/door/LeftDoor2.png"
      "img/door/LeftDoor3.png"
      "img/xmax/xMaxRightCorridor.png"
      "img/xmax/xMaxLeftBackStage.png"
      "img/xmax/xMaxLeftCorridor.png"
      "img/xmax/xMaxRightStage.png"
      "img/xmax/xMaxBossOffice.png"
      "img/xmax/xMaxLeftStage.png"
      "img/xmax/xMaxRightHall.png"
      "img/xmax/xMaxRightDoor.png"
      "img/xmax/xMaxEntryRoom.png"
      "img/xmax/xMaxLeftDoor.png"
      "img/xmax/xMaxLeftHall.png"
      "img/xmax/xMaxKitchen.png"
      "img/xmax/xMaxRightBackStage.png"
      "img/xmax/xMaxStare.png"]
    
let inline setCanvasSize () =
    gl.canvas.height <- 1080
    gl.canvas.width <- 1920
    
setCanvasSize ()
bindButtons ()

loadTextures gl textures (fun texturesMap ->
let rData = { gl = gl; textures = texturesMap }
prepare rData
gameLoop initGameState rData draw 0.)
