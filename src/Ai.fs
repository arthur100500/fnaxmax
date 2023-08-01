module Ai

open Locations
open System

type xmaxtype =
    | One
    | Two

let getNextRooms leftDoor rightDoor =
    function
    | RightDoor -> [ if not rightDoor then SecurityOffice else CenterBackStage ]
    | LeftDoor -> [ if not leftDoor then SecurityOffice else CenterBackStage ]
    | CenterBackStage -> [ RightBackStage; LeftBackStage ]
    | RightCorridor -> [ Kitchen; RightDoor; BossOffice ]
    | Kitchen -> [ RightCorridor; LeftCorridor ]
    | EntryRoom -> [ BossOffice; RightCorridor ]
    | RightHall -> [ RightCorridor; EntryRoom ]
    | LeftHall -> [ RightHall; LeftCorridor ]
    | LeftCorridor -> [ Kitchen; LeftDoor ]
    | SecurityOffice -> [ SecurityOffice ]
    | RightBackStage -> [ RightStage ]
    | LeftBackStage -> [ LeftStage ]
    | RightStage -> [ RightHall ]
    | BossOffice -> [ RightDoor ]
    | Vent -> [ SecurityOffice ]
    | LeftStage -> [ LeftHall ]

let getLocationDuration =
    function
    | LeftCorridor -> 40
    | RightCorridor -> 40
    | EntryRoom -> 20
    | Kitchen -> 70
    | RightHall -> 30
    | LeftHall -> 30
    | CenterBackStage -> 30
    | BossOffice -> 20
    | RightBackStage -> 50
    | LeftBackStage -> 50
    | RightStage -> 80
    | LeftStage -> 80
    | RightDoor -> 20
    | LeftDoor -> 20
    | Vent -> 150
    | SecurityOffice -> 1000

type xmax =
    { t: xmaxtype
      location: location
      dur: int }

let makeStep (xmaxloc: xmax) (leftClosed: bool) (rightClosed: bool) : xmax =
    let rand = Random()

    let pickRandom lst =
        List.item (rand.Next() % (List.length lst)) lst

    match xmaxloc.dur with
    | 0 ->
        let newLoc = getNextRooms leftClosed rightClosed xmaxloc.location |> pickRandom
        let newDur = getLocationDuration newLoc

        { xmaxloc with
            location = newLoc
            dur = newDur }
    | n -> { xmaxloc with dur = n - 1 }

let printXmax xmax =
    printfn "%s" (string xmax)

let initialxmaxes =
    [ { t = One
        location = RightStage
        dur = 30 }
      { t = Two
        location = LeftStage
        dur = 20 } ]
