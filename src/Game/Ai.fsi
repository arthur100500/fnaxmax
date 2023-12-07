module Ai

open Locations

type xMaxType
type xMax =
    { t: xMaxType
      location: location
      dur: int }
val initialXMaxes: xMax list
val makeStep: xMax -> bool -> bool -> xMax
val printXMax: 'a -> unit