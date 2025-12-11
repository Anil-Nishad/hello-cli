using System;

public static class ExecTrace
{
    public static void Main()
    {
        Console.WriteLine("Start");             // compile C# -> IL (build time)
        Console.WriteLine(Hot(2, 3));           // first call -> JIT compiles Hot
        long s = 0;
        for (int i = 0; i < 1_000_000; i++)
            s += Hot(i, i);
        Console.WriteLine(s);                   // hot path (may tier-up)
        // Metadata is embedded in the assembly; IL was verified at load/JIT time.
    }

    static int Hot(int a, int b) => a + b;
}


/*


I'll (1) list the compilation/runtime steps, (2) show where metadata & verification occur, and (3) map that flow onto your Program.cs example with a few commands you can run.

Steps

Source → Compile: C# source → IL + metadata (build time).
Load: CLR loader reads PE file, reads metadata and IL (process start / assembly load).
Verify: JIT-time (or loader-time) IL verification checks type-safety and metadata consistency.
JIT: Methods JIT-compiled to native when first invoked (or by tiered/JIT optimizations).
Native Execution: CPU executes the generated native code; runtime may recompile hotter methods with stronger optimizations (tiering).
Optional AOT/R2R: Ahead-of-time (ReadyToRun) or Native images can skip JIT for some methods.
Where metadata & verification fit

Metadata: Embedded in the assembly (manifest, type/method signatures, attributes, references). Used by the loader, binder, reflection, and the JIT to know method signatures, types, IL offsets, and permissions.
Verification: Occurs when IL is validated for safety/consistency (mostly at JIT time for Core CLR). Verification uses metadata to ensure IL respects the type system and calling/construction rules. If verification fails (or code uses unverifiable constructs), JIT may still compile if allowed (unsafe/untrusted scenarios) or throw.
Mapping to your program

File: Program.cs

Compile (build time):

dotnet build turns Program.cs into a PE assembly (DLL/EXE) that contains IL for Main and Hot, plus metadata describing types and method signatures.
The comment in the file "compile C# -> IL (build time)" corresponds to this step.
Load + Metadata:

When you dotnet run the CLR loads the assembly. The loader reads the metadata (type ExecTrace, methods Main and Hot, method tokens, etc.). Metadata is used to resolve Console.WriteLine, Hot, and to wire up references.
First-call JIT & Verification:

First time Hot is called (via Console.WriteLine(Hot(2, 3))) the JIT compiles the IL body of Hot into native code. Before or during that JIT compilation, the IL for Hot is validated (verified) against metadata—ensuring e.g., the types and stack use are correct.
So the second line in Main ("first call -> JIT compiles Hot") is exactly when IL → native happens for Hot.
Hot path / tiering:

The for loop that calls Hot a million times warms up the method. The CLR may apply tiered compilation: initially compile a quick baseline native version, then later recompile Hot with more aggressive optimizations once it’s identified as hot (tier-up). That maps to your comment "hot path (may tier-up)".
Native execution and subsequent calls:

After JIT, subsequent calls to Hot execute the native code (no IL interpretation). Any re-JIT for optimizations replaces the native code with a faster version.

*/