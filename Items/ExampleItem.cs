using BepInEx.Configuration;
using BepInEx.Logging;
using R2API;
using R2API.Utils;
using RoR2;
using RoRLoL.Utils;
using UnityEngine;
using static RoRLoL.Main;

namespace RoRLoL.Items
{
    public class ExampleItem : ItemBase<ExampleItem>
    {
        public override string ItemName => "Trinity Force";

        public override string ItemLangTokenName => "TRINITY_FORCE";

        public override string ItemPickupDesc => "Tons of DAMAGE";

        public override string ItemFullDescription => "Gains attack speed and base damage after each hit (Max 3)";

        public override string ItemLore => "Tons of DAMAGE";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("trinity_force.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("trinity_force.png");

        public static GameObject ItemBodyModelPrefab;

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        public override void CreateConfig(ConfigFile config)
        {

        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = ItemModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemHelpers.ItemDisplaySetup(ItemBodyModelPrefab);
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();

            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0,0,0),
                    localAngles = new Vector3(0,0,0),
                    localScale = new Vector3(1,1,1),
                }
            });

            return rules;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            var inventory = sender.inventory;
            if (inventory)
            {
                int stack = inventory.GetItemCount(ItemDef);
                if (stack > 0)
                {
                    args.moveSpeedMultAdd += 1f * stack;
                }
            }
        }
    }
}
