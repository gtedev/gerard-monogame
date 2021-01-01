namespace GerardMonogame.Game

[<AutoOpen>]
module GameActivePatterns =

    open GerardMonogame.Game.Types

    let (|SomeBonhommeEntity|_|) (someEntity: IGameEntity option) =
        match someEntity with
        | Some entity ->
            match entity.CustomEntityProperties with
            | Some (BonhommeProperties properties) -> Some((entity.Properties, properties))
            | _ -> None
        | _ -> None
