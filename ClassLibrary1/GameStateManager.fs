module GameStateManager

 open Types

 let updateEntity gameTime gameEntity = 
    match gameEntity.entityType with 
    | BonhommeEntity -> BonhommeEntity.update gameTime gameEntity
    | _ -> gameEntity

 let updateEntities gameTime gameState:GameState = 
     let newEntities = 
      gameState.entities 
      |> List.map (updateEntity gameTime)

     { entities = newEntities }

