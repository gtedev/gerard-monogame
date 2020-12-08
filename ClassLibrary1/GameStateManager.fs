module GameStateManager

 open Types

 let updateEntities gameTime gameState:GameState = 
     let newEntities = 
      gameState.entities 
      |> List.map (fun entity -> entity.updateEntity gameTime entity)

     { entities = newEntities }

