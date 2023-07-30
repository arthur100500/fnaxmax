module Game

open CameraState
open Browser
open Ai

type doorStage = int

type doorStatus =
    | Open
    | Closed
    | Opening of doorStage
    | Closing of doorStage

type userControls =
    { mutable cameraSwitch: bool
      mutable cameraLeft: bool
      mutable cameraRight: bool
      mutable lightLeft: bool
      mutable lightRight: bool }

type gameState =
    { pov: pointOfView
      leftDoor: doorStatus
      rightDoor: doorStatus
      time: float
      tickNum: int
      enemies: xmax list
      leftLight: bool
      rightLight: bool }

let initGameState =
    { pov = CamLeftCorridor, InOffice
      leftDoor = Open
      rightDoor = Open
      time = 0.
      tickNum = 0
      enemies = initialxmaxes
      leftLight = false
      rightLight = false }

let globalControls =
    { cameraSwitch = false
      cameraLeft = false
      cameraRight = false
      lightLeft = false
      lightRight = false }

let tickLen = 50.

let rec gameLoop gameState renderData draw dt =
    let tick = gameState.tickNum < (int <| dt / tickLen)

    let inline switchCamera g =
        let c g =
            globalControls.cameraSwitch <- false
            g

        match gameState.pov, globalControls.cameraSwitch with
        | _, false -> c g
        | (l, InOffice), _ -> c { g with pov = (l, InCameras) }
        | (l, InCameras), _ -> { c g with pov = (l, InOffice) }

    let inline camLeft g =
        let c g =
            globalControls.cameraLeft <- false
            g

        match gameState.pov, gameState.leftDoor, globalControls.cameraLeft with
        | _, _, false -> g
        | (_, InOffice), Open, _ -> c { g with leftDoor = Closing 0 }
        | (_, InOffice), Closed, _ -> c { g with leftDoor = Opening 3 }
        | (l, InCameras), _, _ -> c { g with pov = (moveLeft l, InCameras) }
        | _, _, _ -> c g


    let inline camRight g =
        let c g =
            globalControls.cameraRight <- false
            g

        match gameState.pov, gameState.rightDoor, globalControls.cameraRight with
        | _, _, false -> g
        | (_, InOffice), Open, _ -> c { g with rightDoor = Closing 0 }
        | (_, InOffice), Closed, _ -> c { g with rightDoor = Opening 3 }
        | (l, InCameras), _, _ ->
            { g with
                pov = (moveRight l, InCameras) }
            |> c
        | _, _, _ -> c g

    let doorTick =
        if tick then
            function
            | Opening x when x > 1 -> Opening(x - 1)
            | Closing x when x < 3 -> Closing(x + 1)
            | Opening _ -> Open
            | Closing _ -> Closed
            | e -> e
        else
            id

    let makeStepAll xmaxes =
        if tick && gameState.tickNum % 10 = 0 then
            List.map makeStep xmaxes
        else
            xmaxes

    let gameState =
        { gameState with
            leftDoor = doorTick gameState.leftDoor
            rightDoor = doorTick gameState.rightDoor }

    let gameState = switchCamera gameState
    let gameState = camLeft gameState
    let gameState = camRight gameState

    let gameState =
        { gameState with
            leftLight = globalControls.lightLeft
            rightLight = globalControls.lightRight }

    let gameState =
        { gameState with
            enemies = makeStepAll gameState.enemies }

    if tick && gameState.tickNum % 10 = 0 then
        List.iter printXmax gameState.enemies

    let gameState =
        { gameState with
            time = dt / 1000.
            tickNum = int (dt / tickLen) }

    draw gameState renderData

    window.requestAnimationFrame (gameLoop gameState renderData draw) |> ignore
