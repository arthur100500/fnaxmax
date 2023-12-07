module FiveNightsAtxMax_.Start

open Fable.Core
open Audio
open Game
open Textures
open Render
open RenderBasics
open Browser
open Browser.Types
open GameBasics

let [<Global("Audio")>] audioType : HTMLAudioElementType = jsNative
open Fable.Core.JsInterop

let inline bindButtons () =
    let lightRight = document.getElementById "light-right"
    let lightLeft = document.getElementById "light-left"
    let camSwitch = document.getElementById "camera-up"
    let camRight = document.getElementById "camera-right"
    let camLeft = document.getElementById "camera-left"
    lightRight.onclick <- fun _ ->
        globalControls.lightRight <- not globalControls.lightRight
        if globalControls.lightRight then playAudio LightOn
        else playAudio LightOff
    lightLeft.onclick <- fun _ ->
        globalControls.lightLeft <- not globalControls.lightLeft
        if globalControls.lightLeft then playAudio LightOn
        else playAudio LightOff
    camSwitch.onclick <- fun _ -> globalControls.cameraSwitch <- true
    camRight.onclick <- fun _ -> globalControls.cameraRight <- true
    camLeft.onclick <- fun _ -> globalControls.cameraLeft <- true
    
let gl: WebGLRenderingContext =
    let canvas: Types.HTMLCanvasElement = unbox document.getElementById "game-canvas"
    unbox canvas.getContext "webgl"

let inline setCanvasSize () =
    gl.canvas.height <- 1080
    gl.canvas.width <- 1920
    
let inline startAmbient () =
    playAudioLoop Ambient
  
let inline showGame () =
    let gameDiv = document.getElementById "game"
    gameDiv?style?visibility <- "visible"

let start () =
    setCanvasSize ()
    bindButtons ()
    showGame ()

    loadTextures gl (fun texturesMap ->
    let rData = { gl = gl; textures = texturesMap }
    prepare rData
    startAmbient () 
    gameLoop initGameState rData 0.)
