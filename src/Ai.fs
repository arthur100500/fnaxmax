module Ai

open Locations
open System

type xmaxtype =
    | One
    | Two

let getNextRooms =
    function
    | LeftCorridor -> [ Kitchen; LeftDoor ]
    | RightCorridor -> [ Kitchen; RightDoor; BossOffice ]
    | EntryRoom -> [ BossOffice; RightCorridor ]
    | Kitchen -> [ Vent; RightCorridor; LeftCorridor ]
    | RightHall -> [ RightCorridor; EntryRoom ]
    | LeftHall -> [ RightHall; LeftCorridor ]
    | CenterBackStage -> [ RightBackStage; LeftBackStage ]
    | BossOffice -> [ RightDoor ]
    | RightBackStage -> [ RightStage ]
    | LeftBackStage -> [ LeftStage ]
    | RightStage -> [ RightHall ]
    | LeftStage -> [ LeftHall ]
    | RightDoor -> [ SecurityOffice ]
    | LeftDoor -> [ SecurityOffice ]
    | Vent -> [ SecurityOffice ]
    | SecurityOffice -> [ SecurityOffice ]

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

type xmax = xmaxtype * location * int

let makeStep (xmaxloc: xmax) : xmax =
    let rand = Random()

    let pickRandom lst =
        List.item (rand.Next() % (List.length lst)) lst

    let mtype, loc, dur = xmaxloc

    match dur with
    | 0 ->
        let newLoc = getNextRooms loc |> pickRandom
        let newDur = getLocationDuration newLoc
        mtype, newLoc, newDur
    | n -> mtype, loc, n - 1

let printXmax xmax =
    let t, loc, dur = xmax
    printfn "%s %s [%d]" (string t) (string loc) dur

let initialxmaxes = [ One, RightStage, 30; Two, LeftStage, 20 ]
