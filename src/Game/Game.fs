module Game

open CameraState
open Browser
open Ai
open Audio
open GameBasics
open GameRender

let tickLen = 50.

(* Control methods *)
let inline cameraSwitchPress () =
    match globalControls.cameraSwitch with
    | true ->
        globalControls.cameraSwitch <- false
        true
    | false -> false

let inline cameraLeftPress () =
    match globalControls.cameraLeft with
    | true ->
        globalControls.cameraLeft <- false
        true
    | false -> false

let inline cameraRightPress () =
    match globalControls.cameraRight with
    | true ->
        globalControls.cameraRight <- false
        true
    | false -> false


(* Game Loop *)

let rec gameLoop gameState renderData dt =
    let tick = gameState.tickNum < (int <| dt / tickLen)

    let inline switchCamera g =
        match gameState.pov, cameraSwitchPress () with
        | _, false -> g
        | (l, InOffice), _ -> { g with pov = (l, InCameras) }
        | (l, InCameras), _ -> { g with pov = (l, InOffice) }

    let inline camLeft g =
        match gameState.pov, gameState.leftDoor, cameraLeftPress () with
        | _, _, false -> g
        | (_, InOffice), Open, _ ->
            playAudio DoorClose
            { g with leftDoor = Closing 0 }
        | (_, InOffice), Closed, _ ->
            playAudio DoorOpen
            { g with leftDoor = Opening 3 }
        | (l, InCameras), _, _ -> { g with pov = (moveLeft l, InCameras) }
        | _, _, _ -> g


    let inline camRight g =
        match gameState.pov, gameState.rightDoor, cameraRightPress () with
        | _, _, false -> g
        | (_, InOffice), Open, _ ->
            playAudio DoorClose
            { g with rightDoor = Closing 0 }
        | (_, InOffice), Closed, _ ->
            playAudio DoorOpen
            { g with rightDoor = Opening 3 }
        | (l, InCameras), _, _ ->
            { g with
                pov = (moveRight l, InCameras) }
        | _, _, _ -> g

    let inline doorTick x =
        if tick then
            match x with
            | Opening x when x > 1 -> Opening(x - 1)
            | Closing x when x < 3 -> Closing(x + 1)
            | Opening _ -> Open
            | Closing _ -> Closed
            | e -> e
        else
            x

    let inline makeStepAll xMaxes =
        if tick && gameState.tickNum % 10 = 0 then
            List.map (fun x -> makeStep x (gameState.leftDoor = Closed) (gameState.rightDoor = Closed)) xMaxes
        else
            xMaxes

    let inline doorTick gameState =
        { gameState with
            leftDoor = doorTick gameState.leftDoor
            rightDoor = doorTick gameState.rightDoor }

    let inline setLights gameState =
        { gameState with
            leftLight = globalControls.lightLeft
            rightLight = globalControls.lightRight }

    let inline xMaxStep gameState =
        { gameState with
            enemies = makeStepAll gameState.enemies }

    let inline updateTime gameState =
        { gameState with
            time = dt / 1000.
            tickNum = int (dt / tickLen) }

    let gameState =
        gameState
        |> doorTick
        |> switchCamera
        |> camLeft
        |> camRight
        |> setLights
        |> xMaxStep
        |> updateTime

    renderRoom gameState renderData

    window.requestAnimationFrame (gameLoop gameState renderData)
    |> ignore
