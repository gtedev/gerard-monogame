module KeyboardState

open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework

let (|KeyDown|_|) k (state: KeyboardState) =
    if state.IsKeyDown k then Some() else None

let getMovementVector keyState speed = 

    let speedVector = new Vector2(speed,speed)

    let normalVector = match keyState with
    | KeyDown Keys.Right & KeyDown Keys.Up   -> Vector2(1.f, -1f)
    | KeyDown Keys.Right & KeyDown Keys.Down -> Vector2(1.f, 1f)
    | KeyDown Keys.Up -> Vector2(0f, -1.f)
    | KeyDown Keys.Down  -> Vector2(0f, 1.f)
    | KeyDown Keys.Right  -> Vector2(1.f, 0f)
    | KeyDown Keys.Left  -> Vector2(-1.f, 0f)
    | _ -> Vector2.Zero

    Vector2.Multiply(speedVector, normalVector)