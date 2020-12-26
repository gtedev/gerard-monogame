[<AutoOpen>]
module Helper
/// <summary>Update first element from doublet tuple.</summary>
let updateFst2 (x: 'a) (a: 'a, b) = (x, b)

/// <summary>Update second element from doublet tuple.</summary>
let updateSnd2 (x: 'a) (a, b: 'a) = (a, x)




/// <summary>Get first element from triplet tuple.</summary>
let fst3 (a, b, c) = a

/// <summary>Get second element from triplet tuple.</summary>
let snd3 (a, b, c) = b

/// <summary>Get third element from triplet tuple.</summary>
let thrd3 (a, b, c) = c

/// <summary>Update first element from triplet tuple.</summary>
let updateFst3 (x: 'a) (a: 'a, b, c) = (x, b, c)

/// <summary>Update second element from triplet tuple.</summary>
let updateSnd3 (x: 'a) (a, b: 'a, c) = (a, x, c)

/// <summary>Update third element from triplet tuple.</summary>
let updateThrd3 (x: 'a) (a, b, c: 'a) = (a, b, x)
