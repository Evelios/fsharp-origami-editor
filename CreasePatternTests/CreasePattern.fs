module CreasePatternTests.CreasePattern

open NUnit.Framework
open CreasePattern

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Compilation () =
    let creasePattern = CreasePattern.empty
    Assert.Pass()