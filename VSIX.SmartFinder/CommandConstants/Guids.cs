// Guids.cs
// MUST match guids.h
using System;

namespace Geeks.VSIX.SmartFinder
{
    static class GuidList
    {
        public const string GuidGeeksProductivityToolsPkgString = "c6176957-c61c-4beb-8dd8-e7c0170b0bf4";

        const string GUID_COMPILE_TS_CMD_SET_STRING = "b280ef5d-91d2-43b7-a482-224d04bcb2ef";
        const string GUID_MSHARP_EDITOR_CMD_SET_STRING = "5b066d90-2a03-4176-81cb-a88921683988";
        const string GUID_RUN_BATCH_FILE_CMD_SET_STRING = "af7f4c9e-13a9-4081-b87a-e5016ad1a301";
        const string GUID_ORGANIZE_USING_CMD_SET_STRING = "e3b37086-ac68-4a62-b7a4-f9f68e1dda61";
        const string GUID_TRIM_BLANK_LINES_CMD_SET_STRING = "9554734e-0c47-4173-96f0-466c505fc3cd";
        const string GUID_OPEN_IN_MSHARP_SLN_CMD_SET_STRING = "54fb8f31-e4d8-4258-a86d-5b35af2a6208";
        const string GUID_ORGANIZE_USING_SLN_LEVEL_CMD_SET_STRING = "932ac1b0-4cfb-4222-a296-ed84df197a64";
        const string GUID_GEEKS_PRODUCTIVITY_TOOLS_CMD_SET_STRING = "8d55b43e-5f7c-44dd-8b02-71c751d8c440";
        const string GUID_CLEANUP_CMD_SET_STRING = "53366ba1-1788-42c8-922a-034d6dc89b12";
        //
        public static readonly Guid GuidCompileTsCmdSet = new Guid(GUID_COMPILE_TS_CMD_SET_STRING);
        public static readonly Guid GuidMSharpEditorCmdSet = new Guid(GUID_MSHARP_EDITOR_CMD_SET_STRING);
        public static readonly Guid GuidRunBatchFileCmdSet = new Guid(GUID_RUN_BATCH_FILE_CMD_SET_STRING);
        public static readonly Guid GuidOrganizeUsingCmdSet = new Guid(GUID_ORGANIZE_USING_CMD_SET_STRING);

        public static readonly Guid GuidTrimBlankLinesCmdSet = new Guid(GUID_TRIM_BLANK_LINES_CMD_SET_STRING);
        public static readonly Guid GuidOpenInMSharpSlnCmdSet = new Guid(GUID_OPEN_IN_MSHARP_SLN_CMD_SET_STRING);
        public static readonly Guid GuidOrganizeUsingSlnLevelCmdSet = new Guid(GUID_ORGANIZE_USING_SLN_LEVEL_CMD_SET_STRING);
        public static readonly Guid GuidGeeksProductivityToolsCmdSet = new Guid(GUID_GEEKS_PRODUCTIVITY_TOOLS_CMD_SET_STRING);

        public static readonly Guid GuidCleanupCmdSet = new Guid(GUID_CLEANUP_CMD_SET_STRING);
    };
}