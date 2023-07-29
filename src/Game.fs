module Game

open CameraState
open Browser
open Browser.Types
open Locations

type doorStage = int

type doorStatus =
    | Open
    | Closed
    | Opening of doorStage
    | Closing of doorStage

type userControls =
    { mutable cameraSwitch: bool
      mutable cameraLeft: bool
      mutable cameraRight: bool }

type gameState =
    { pov: pointOfView
      leftDoor: doorStatus
      rightDoor: doorStatus
      time: float }

let initGameState =
    { pov = CamLeftCorridor, InOffice
      leftDoor = Open
      rightDoor = Open
      time = 0. }

let globalControls =
    { cameraSwitch = false
      cameraLeft = false
      cameraRight = false }

let rec gameLoop gameState renderData draw dt =

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
        | (_, InOffice), Open, _ -> c { g with leftDoor = Closing 3 }
        | (_, InOffice), Closed, _ -> c { g with leftDoor = Opening 0 }
        | (l, InCameras), _, _ -> c { g with pov = (moveLeft l, InCameras) }
        | _, _, _ -> c g


    let inline camRight g =
        let c g =
            globalControls.cameraRight <- false
            g

        match gameState.pov, gameState.rightDoor, globalControls.cameraRight with
        | _, _, false -> g
        | (_, InOffice), Open, _ -> c { g with rightDoor = Closing 3 }
        | (_, InOffice), Closed, _ -> c { g with rightDoor = Opening 0 }
        | (l, InCameras), _, _ -> { g with pov = (moveRight l, InCameras) } |> c
        | _, _, _ -> c g

    let doorTick =
        function
        | Opening x when x > 1 -> Opening(x - 1)
        | Closing x when x < 3 -> Closing(x + 1)
        | Opening _ -> Open
        | Closing _ -> Closed
        | e -> e

    let gameState =
        { gameState with
            leftDoor = doorTick gameState.leftDoor
            rightDoor = doorTick gameState.rightDoor }

    let gameState = switchCamera gameState
    let gameState = camLeft gameState
    let gameState = camRight gameState

    let gameState = { gameState with time = dt / 1000. }

    console.log gameState
    
    draw gameState renderData

    window.requestAnimationFrame (gameLoop gameState renderData draw) |> ignore
