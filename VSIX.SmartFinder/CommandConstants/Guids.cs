// Guids.cs
// MUST match guids.h
using System;

namespace Geeks.VSIX.SmartFinder
{
    static class GuidList
    {
        public const string GuidGeeksProductivityToolsPkgString = "c6176957-c61c-4beb-8dd8-e7c0170b0bf4";

        const string guidCompileTsCmdSetString = "b280ef5d-91d2-43b7-a482-224d04bcb2ef";
        const string guidMSharpEditorCmdSetString = "5b066d90-2a03-4176-81cb-a88921683988";
        const string guidRunBatchFileCmdSetString = "af7f4c9e-13a9-4081-b87a-e5016ad1a301";
        const string guidOrganizeUsingCmdSetString = "e3b37086-ac68-4a62-b7a4-f9f68e1dda61";
        const string guidTrimBlankLinesCmdSetString = "9554734e-0c47-4173-96f0-466c505fc3cd";
        const string guidOpenInMSharpSlnCmdSetString = "54fb8f31-e4d8-4258-a86d-5b35af2a6208";
        const string guidOrganizeUsingSlnLevelCmdSetString = "932ac1b0-4cfb-4222-a296-ed84df197a64";
        const string guidGeeksProductivityToolsCmdSetString = "8d55b43e-5f7c-44dd-8b02-71c751d8c440";
        const string guidCleanupCmdSetString = "53366ba1-1788-42c8-922a-034d6dc89b12";
        //
        public static readonly Guid GuidCompileTsCmdSet = new Guid(guidCompileTsCmdSetString);
        public static readonly Guid GuidMSharpEditorCmdSet = new Guid(guidMSharpEditorCmdSetString);
        public static readonly Guid GuidRunBatchFileCmdSet = new Guid(guidRunBatchFileCmdSetString);
        public static readonly Guid GuidOrganizeUsingCmdSet = new Guid(guidOrganizeUsingCmdSetString);

        public static readonly Guid GuidTrimBlankLinesCmdSet = new Guid(guidTrimBlankLinesCmdSetString);
        public static readonly Guid GuidOpenInMSharpSlnCmdSet = new Guid(guidOpenInMSharpSlnCmdSetString);
        public static readonly Guid GuidOrganizeUsingSlnLevelCmdSet = new Guid(guidOrganizeUsingSlnLevelCmdSetString);
        public static readonly Guid GuidGeeksProductivityToolsCmdSet = new Guid(guidGeeksProductivityToolsCmdSetString);

        public static readonly Guid GuidCleanupCmdSet = new Guid(guidCleanupCmdSetString);
    };
}