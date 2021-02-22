namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open Microsoft.Xna.Framework.Input
open GerardMonogame.Constants

[<RequireQualifiedAccess>]
module Level1Update =
    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.Level1Constants



    let private ``make sure level1 never moves too much to the right`` posX (vectorMov: Vector2) =

        let nextXPos =
            if posX + vectorMov.X > 0f then 0f else vectorMov.X

        new Vector2(nextXPos, vectorMov.Y)



    let private ``update level1 movement in opposite direction of bonhomme`` (bonhommeProps: BonhommeProperties)
                                                                             (vectorMov: Vector2)
                                                                             =
        let idleLvl1Vector = new Vector2(0f, 0f)

        let ``and bonhomme is after minimum X Position`` =
            bonhommeProps.position.X > LEVEL1_BONHOMME_X_POSITION_MOVE_TRIGGER

        let nextMoveLvl1Vector dir =
            // move of the opposite direction
            let nextXMovPos = GameHelper.matchDirection (1f) (-1f) dir

            new Vector2(nextXMovPos * SPEED_MOVING_FLOOR, vectorMov.Y)


        match bonhommeProps.movementStatus with
        | Duck _ -> idleLvl1Vector
        | Jumping (Toward dir, _) when ``and bonhomme is after minimum X Position`` ->

            nextMoveLvl1Vector dir

        | Running dir when ``and bonhomme is after minimum X Position`` ->

            nextMoveLvl1Vector dir

        | _ -> idleLvl1Vector



    let private ``make sure level1 never moves vertically`` (vectorMov: Vector2) =
        // maintain level background to same Y position !
        new Vector2(vectorMov.X, 0f)



    let private updateLevel1Entity (bonhommeProps: BonhommeProperties)
                                   (currentEntity: GameEntity)
                                   (lvl1Props: Level1Properties)
                                   =

        let nextVectorPos =
            new Vector2(0f, 0f)
            |> ``update level1 movement in opposite direction of bonhomme`` bonhommeProps
            |> ``make sure level1 never moves vertically``
            |> ``make sure level1 never moves too much to the right`` ((List.head lvl1Props.positions).X)


        let ``remove sprite when it goes off screen and queue new sprite`` (list: SpritePosition list) =
            let first = List.head list
            
            match first.X with 
            | posX when posX < -2476f ->  List.tail list |> (fun  ls -> List.append (ls) ([new Vector2(2476f, LEVEL1_Y_POSITION)]))
            | _ -> list


        let nextPosList =
            lvl1Props.positions
            |> List.map (fun pos -> Vector2.Add(pos, nextVectorPos))
            |> ``remove sprite when it goes off screen and queue new sprite``

        let nextLvl1Props =
            Level1Properties(
                { lvl1Props with
                      positions = nextPosList }
            )



        let nextSprite =

            let createSpriteProps pos =
                { texture = lvl1Props.spriteSheet.level1Sprite
                  position = pos }

            let spriteProps =
                nextPosList |> List.map createSpriteProps

            GroupOfSprites spriteProps


        currentEntity
        |> GameEntity.updateEntity nextSprite nextLvl1Props



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity) (lvl1Props: Level1Properties) =

        let bhEntity =
            GameEntity.tryGetEntity gs (GameEntityId BonhommeConstants.EntityId)

        match bhEntity with
        | SomeBonhomme props ->

            updateLevel1Entity props currentEntity lvl1Props

        | _ -> currentEntity
