using Dalamud.Game.ClientState.Objects.SubKinds;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System.Collections.Generic;
using System.Linq;

namespace AutoNaelMarker.Classes;

public static unsafe class Helper
{
    public const string SettingsCommand = "/nmsettings";
    public const string PriorityCommand = "/nmpriority";
    public const uint CollectionTimeout = 15000;
    public const int LightningCount = 2;
    public const int JobCount = 21;
    public static Dictionary<int, string> Classes { get; set; } = new();
    public static bool IsMarking { get; set; }

    public static readonly string[] MarkPrefix = { "First", "Second", "Third" };
    public const string MarkMessage = " mark: {0} - Party list position: {1}";
    public const string NotInPrioMessage = "Not in priority list - using party list as priority";

    public static bool PlayerExists => Service.ClientState?.LocalPlayer != null;

    public static bool IsIdInParty(ulong id)
    {
        if (!PlayerExists) return false;
        // 733 = UCOB
        return Service.ClientState.TerritoryType is 733 &&
               Service.PartyList.Any(p => p.GameObject?.GameObjectId == (uint)id);
    }

    public static int GetHudGroupMember(int index)
    {
        var frameworkInstance = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance();
        var baseAddress = (byte*)frameworkInstance->UIModule->GetAgentModule()->GetAgentByInternalId(AgentId.Hud);
        const int groupDataOffset = 0xCC8;

        var objectId = *(int*)(baseAddress + groupDataOffset + index * 0x20 + 0x18);

        return objectId;
    }

    public static IPlayerCharacter GetPlayerByObjectId(uint objectId)
    {
        var result = Service.ObjectTable.SearchById(objectId);

        if (result?.GetType() == typeof(IPlayerCharacter) && result as IPlayerCharacter != null)
            return result as IPlayerCharacter;

        return null;
    }
}