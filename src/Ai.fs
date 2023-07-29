module Ai

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
        | LeftCorridor -> 4
        | RightCorridor -> 4
        | EntryRoom -> 2
        | Kitchen -> 7
        | RightHall -> 3
        | LeftHall -> 3
        | CenterBackStage -> 3
        | BossOffice -> 2
        | RightBackStage -> 5
        | LeftBackStage -> 5
        | RightStage -> 8
        | LeftStage -> 8
        | RightDoor -> 20
        | LeftDoor -> 20
        | Vent -> 15
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