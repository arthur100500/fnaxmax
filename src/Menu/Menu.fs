module FiveNightsAtxMax_.Menu

open Browser
open Start
open Fable.Core.JsInterop

let inline bindButtons () =
    let startButton = document.getElementById "start"
    let menuDiv = document.getElementById "menu"
    startButton.onclick <- fun _ ->
        menuDiv?style?visibility <- "hidden"
        start()
    
let startMenu () =
    bindButtons ()