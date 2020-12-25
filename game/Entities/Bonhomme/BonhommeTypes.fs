module BonhommeTypes

open Types
open Microsoft.Xna.Framework

type NextBonhommeMovement = (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option)
