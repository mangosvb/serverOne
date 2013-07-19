' 
' Copyright (C) 2013 getMaNGOS <http://www.getMangos.co.uk>
'
' This program is free software; you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation; either version 2 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program; if not, write to the Free Software
' Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
'
Imports MySql.Data.MySqlClient
Imports mangosVB.Common
Public Class NCDBcreatures
#Region "Variables"
    'Misc Variables
    'Declaring variables for the NCDB creature table 
    'Creature_root
    Dim v_name As String = " "
    Dim v_subname As String = " "
    Dim v_info_str As Integer = 0
    Dim v_Flags1 As Integer = 0
    Dim v_type As Integer = 0
    Dim v_family As Integer = 0
    Dim v_rank As Integer = 0
    Dim v_unk4 As Integer = 0
    Dim v_spelldataid As Integer = 0
    Dim v_displayid As Integer = 0
    Dim v_displayid2 As Integer = 0
    Dim v_displayid3 As Integer = 0
    Dim v_displayid4 As Integer = 0
    Dim v_unknown_float1 As Integer = 0
    Dim v_unknown_float2 As Integer = 0
    Dim v_leader As Integer
    'Creature_template
    Dim vs_Entry As Integer
    Dim vs_MinLvl As Integer = 0
    Dim vs_MaxLvl As Integer = 0
    Dim vs_Faction As Integer = 0
    Dim vs_MinHP As Integer = 0
    Dim vs_MaxHP As Integer = 0
    Dim vs_Mana As Integer = 0
    Dim vs_Scale As Integer = 0
    Dim vs_NpcFlags As Integer = 0
    Dim vs_AttackTime As Integer = 0
    Dim vs_AttackPower As Integer = 0
    Dim vs_RangedAttackPower As Integer = 0
    Dim vs_AttackType As Integer = 0
    Dim vs_MinDmg As Integer = 0
    Dim vs_MaxDmg As Integer = 0
    Dim vs_RangedAttackTime As Integer = 0
    Dim vs_RangedMinDmg As Integer = 0
    Dim vs_RangedMaxDmg As Integer = 0
    Dim vs_EquipmentEntry As Integer = 0
    Dim vs_SpellGroup As Integer = 0
    Dim vs_RespawnTime As Integer = 0
    Dim vs_Armor As Integer = 0
    Dim vs_resistance1 As Integer = 0 'Holy
    Dim vs_resistance2 As Integer = 0 'Fire
    Dim vs_resistance3 As Integer = 0 'Nature
    Dim vs_resistance4 As Integer = 0 'Frost
    Dim vs_resistance5 As Integer = 0 'Shadow
    Dim vs_resistance6 As Integer = 0 'Arcane
    Dim vs_CombatReach As Integer = 0
    Dim vs_BoundingRadius As Integer = 0
    Dim vs_Auras As Integer = 0
    Dim vs_Boss As Integer = 0
    Dim vs_Money As Integer = 0
    Dim vs_InvisibilityType As Integer = 0
    Dim vs_DeathState As Integer = 0
    Dim vs_WalkSpeed As Integer = 0
    Dim vs_RunSpeed As Integer = 0
    Dim vs_FlySpeed As Integer = 0
    Dim vs_LootId As Integer = 0
    Dim vs_SkinLoot As Integer = 0
    Dim vs_PickPocketLoot As Integer = 0
    Dim vs_InhabitType As Integer = 0
    Dim vs_HasRegan As Integer = 0
    Dim vs_RespawnMod As Integer = 0
    Dim vs_ArmorMod As Integer = 0
    Dim vs_DamageMod As Integer = 0
    Dim vs_HealthMod As Integer = 0
    Dim vs_ExtraA9Flags As Integer = 0
    Dim vs_manatype As Integer = 0
    Dim vs_sell As Integer = 0
    Dim vs_aiscript As Integer = 0
    Dim vs_Resistance0_Armor = 0

    'End of NCDB creatures table
#End Region
#Region "Properties"
    Public Property name() As String
        Get
            name = v_name
        End Get
        Set(ByVal Value As String)
            v_name = Value
        End Set
    End Property
    Public Property subname() As String
        Get
            subname = v_subname
        End Get
        Set(ByVal Value As String)
            v_subname = Value
        End Set
    End Property
    Public Property info_str() As Integer
        Get
            info_str = v_info_str
        End Get
        Set(ByVal Value As Integer)
            v_info_str = Value
        End Set
    End Property
    Public Property Flags1() As Integer
        Get
            Flags1 = v_Flags1
        End Get
        Set(ByVal Value As Integer)
            v_Flags1 = Value
        End Set
    End Property
    Public Property type() As Integer
        Get
            type = v_type
        End Get
        Set(ByVal Value As Integer)
            v_type = Value
        End Set
    End Property
    Public Property family() As Integer
        Get
            family = v_family
        End Get
        Set(ByVal Value As Integer)
            v_family = Value
        End Set
    End Property
    Public Property rank() As Integer
        Get
            rank = v_rank
        End Get
        Set(ByVal Value As Integer)
            v_rank = Value
        End Set
    End Property
    Public Property unk4() As Integer
        Get
            unk4 = v_unk4
        End Get
        Set(ByVal Value As Integer)
            v_unk4 = Value
        End Set
    End Property
    Public Property spelldataid() As Integer
        Get
            spelldataid = v_spelldataid
        End Get
        Set(ByVal Value As Integer)
            v_spelldataid = Value
        End Set
    End Property
    Public Property displayid() As Integer
        Get
            displayid = v_displayid
        End Get
        Set(ByVal Value As Integer)
            v_displayid = Value
        End Set
    End Property
    Public Property displayid2() As Integer
        Get
            displayid2 = v_displayid2
        End Get
        Set(ByVal Value As Integer)
            v_displayid2 = Value
        End Set
    End Property
    Public Property displayid3() As Integer
        Get
            displayid3 = v_displayid3
        End Get
        Set(ByVal Value As Integer)
            v_displayid3 = Value
        End Set
    End Property
    Public Property displayid4() As Integer
        Get
            displayid4 = v_displayid4
        End Get
        Set(ByVal Value As Integer)
            v_displayid4 = Value
        End Set
    End Property
    Public Property unknown_float1() As Integer
        Get
            unknown_float1 = v_unknown_float1
        End Get
        Set(ByVal Value As Integer)
            v_unknown_float1 = Value
        End Set
    End Property
    Public Property unknown_float2() As Integer
        Get
            unknown_float2 = v_unknown_float2
        End Get
        Set(ByVal Value As Integer)
            v_unknown_float2 = Value
        End Set
    End Property
    Public Property leader() As Integer
        Get
            leader = v_leader
        End Get
        Set(ByVal Value As Integer)
            v_leader = Value
        End Set
    End Property
    'End of Creature_root table properties; start of creature_template properties
    Public Property Entry() As Integer
        Get
            entry = vs_Entry
        End Get
        Set(ByVal Value As Integer)
            vs_Entry = Value
        End Set
    End Property
    Public Property MinLvl() As Integer
        Get
            MinLvl = vs_MinLvl
        End Get
        Set(ByVal Value As Integer)
            vs_MinLvl = Value
        End Set
    End Property
    Public Property MaxLvl() As Integer
        Get
            MaxLvl = vs_MaxLvl
        End Get
        Set(ByVal Value As Integer)
            vs_MaxLvl = Value
        End Set
    End Property
    Public Property Faction() As Integer
        Get
            Faction = vs_Faction
        End Get
        Set(ByVal Value As Integer)
            vs_Faction = Value
        End Set
    End Property
    Public Property MinHP() As Integer
        Get
            MinHP = vs_MinHP
        End Get
        Set(ByVal Value As Integer)
            vs_MinHP = Value
        End Set
    End Property
    Public Property MaxHP() As Integer
        Get
            MaxHP = vs_MaxHP
        End Get
        Set(ByVal Value As Integer)
            vs_MaxHP = Value
        End Set
    End Property
    Public Property Mana() As Integer
        Get
            Mana = vs_Mana
        End Get
        Set(ByVal Value As Integer)
            vs_Mana = Value
        End Set
    End Property
    Public Property Scale() As Integer
        Get
            Scale = vs_Scale
        End Get
        Set(ByVal Value As Integer)
            vs_Scale = Value
        End Set
    End Property
    Public Property NpcFlags() As Integer
        Get
            NpcFlags = vs_NpcFlags
        End Get
        Set(ByVal Value As Integer)
            vs_NpcFlags = Value
        End Set
    End Property
    Public Property AttackTime() As Integer
        Get
            AttackTime = vs_AttackTime
        End Get
        Set(ByVal Value As Integer)
            vs_AttackTime = Value
        End Set
    End Property
    Public Property AttackPower() As Integer
        Get
            MinLvl = vs_MinLvl
        End Get
        Set(ByVal Value As Integer)
            vs_MinLvl = Value
        End Set
    End Property
    Public Property RangedAttackPower() As Integer
        Get
            RangedAttackPower = vs_RangedAttackPower
        End Get
        Set(ByVal Value As Integer)
            vs_RangedAttackPower = Value
        End Set
    End Property
    Public Property AttackType() As Integer
        Get
            AttackType = vs_AttackType
        End Get
        Set(ByVal Value As Integer)
            vs_AttackType = Value
        End Set
    End Property
    Public Property MinDmg() As Integer
        Get
            MinDmg = vs_MinDmg
        End Get
        Set(ByVal Value As Integer)
            vs_MinDmg = Value
        End Set
    End Property
    Public Property MaxDmg() As Integer
        Get
            MaxDmg = vs_MaxDmg
        End Get
        Set(ByVal Value As Integer)
            vs_MaxDmg = Value
        End Set
    End Property
    Public Property RangedAttackTime() As Integer
        Get
            RangedAttackTime = vs_RangedAttackTime
        End Get
        Set(ByVal Value As Integer)
            vs_RangedAttackTime = Value
        End Set
    End Property
    Public Property RangedMinDmg() As Integer
        Get
            RangedMinDmg = vs_RangedMinDmg
        End Get
        Set(ByVal Value As Integer)
            vs_RangedMinDmg = Value
        End Set
    End Property
    Public Property RangedMaxDmg() As Integer
        Get
            RangedMaxDmg = vs_RangedMaxDmg
        End Get
        Set(ByVal Value As Integer)
            vs_RangedMaxDmg = Value
        End Set
    End Property
    Public Property EquipmentEntry() As Integer
        Get
            EquipmentEntry = vs_EquipmentEntry
        End Get
        Set(ByVal Value As Integer)
            vs_EquipmentEntry = Value
        End Set
    End Property
    Public Property SpellGroup() As Integer
        Get
            SpellGroup = vs_SpellGroup
        End Get
        Set(ByVal Value As Integer)
            vs_SpellGroup = Value
        End Set
    End Property
    Public Property RespawnTime() As Integer
        Get
            RespawnTime = vs_RespawnTime
        End Get
        Set(ByVal Value As Integer)
            vs_RespawnTime = Value
        End Set
    End Property
    Public Property Armor() As Integer
        Get
            Armor = vs_Armor
        End Get
        Set(ByVal Value As Integer)
            vs_Armor = Value
        End Set
    End Property
    Public Property resistance1() As Integer
        Get
            resistance1 = vs_resistance1
        End Get
        Set(ByVal Value As Integer)
            vs_resistance1 = Value
        End Set
    End Property
    Public Property resistance2() As Integer
        Get
            resistance2 = vs_resistance2
        End Get
        Set(ByVal Value As Integer)
            vs_resistance2 = Value
        End Set
    End Property
    Public Property resistance3() As Integer
        Get
            resistance3 = vs_resistance3
        End Get
        Set(ByVal Value As Integer)
            vs_resistance3 = Value
        End Set
    End Property
    Public Property resistance4() As Integer
        Get
            resistance4 = vs_resistance4
        End Get
        Set(ByVal Value As Integer)
            vs_resistance4 = Value
        End Set
    End Property
    Public Property resistance5() As Integer
        Get
            resistance5 = vs_resistance5
        End Get
        Set(ByVal Value As Integer)
            vs_resistance5 = Value
        End Set
    End Property
    Public Property resistance6() As Integer
        Get
            resistance6 = vs_resistance6
        End Get
        Set(ByVal Value As Integer)
            vs_resistance6 = Value
        End Set
    End Property
    Public Property CombatReach() As Integer
        Get
            CombatReach = vs_CombatReach
        End Get
        Set(ByVal Value As Integer)
            vs_CombatReach = Value
        End Set
    End Property
    Public Property BoundingRadius() As Integer
        Get
            BoundingRadius = vs_BoundingRadius
        End Get
        Set(ByVal Value As Integer)
            vs_BoundingRadius = Value
        End Set
    End Property
    Public Property Auras() As Integer
        Get
            Auras = vs_Auras
        End Get
        Set(ByVal Value As Integer)
            vs_Auras = Value
        End Set
    End Property
    Public Property Boss() As Integer
        Get
            Boss = vs_Boss
        End Get
        Set(ByVal Value As Integer)
            vs_Boss = Value
        End Set
    End Property
    Public Property Money() As Integer
        Get
            Money = vs_Money
        End Get
        Set(ByVal Value As Integer)
            vs_Money = Value
        End Set
    End Property
    Public Property InvisibilityType() As Integer
        Get
            InvisibilityType = vs_InvisibilityType
        End Get
        Set(ByVal Value As Integer)
            vs_InvisibilityType = Value
        End Set
    End Property
    Public Property DeathState() As Integer
        Get
            DeathState = vs_DeathState
        End Get
        Set(ByVal Value As Integer)
            vs_DeathState = Value
        End Set
    End Property
    Public Property WalkSpeed() As Integer
        Get
            WalkSpeed = vs_WalkSpeed
        End Get
        Set(ByVal Value As Integer)
            vs_WalkSpeed = Value
        End Set
    End Property
    Public Property RunSpeed() As Integer
        Get
            RunSpeed = vs_RunSpeed
        End Get
        Set(ByVal Value As Integer)
            vs_RunSpeed = Value
        End Set
    End Property
    Public Property FlySpeed() As Integer
        Get
            FlySpeed = vs_FlySpeed
        End Get
        Set(ByVal Value As Integer)
            vs_FlySpeed = Value
        End Set
    End Property
    Public Property LootId() As Integer
        Get
            LootId = vs_LootId
        End Get
        Set(ByVal Value As Integer)
            vs_LootId = Value
        End Set
    End Property
    Public Property SkinLoot() As Integer
        Get
            SkinLoot = vs_SkinLoot
        End Get
        Set(ByVal Value As Integer)
            vs_SkinLoot = Value
        End Set
    End Property
    Public Property PickPocketLoot() As Integer
        Get
            PickPocketLoot = vs_PickPocketLoot
        End Get
        Set(ByVal Value As Integer)
            vs_PickPocketLoot = Value
        End Set
    End Property
    Public Property InhabitType() As Integer
        Get
            InhabitType = vs_InhabitType
        End Get
        Set(ByVal Value As Integer)
            vs_InhabitType = Value
        End Set
    End Property
    Public Property HasRegan() As Integer
        Get
            HasRegan = vs_HasRegan
        End Get
        Set(ByVal Value As Integer)
            vs_HasRegan = Value
        End Set
    End Property
    Public Property RespawnMod() As Integer
        Get
            RespawnMod = vs_RespawnMod
        End Get
        Set(ByVal Value As Integer)
            vs_RespawnMod = Value
        End Set
    End Property
    Public Property ArmorMod() As Integer
        Get
            ArmorMod = vs_ArmorMod
        End Get
        Set(ByVal Value As Integer)
            vs_ArmorMod = Value
        End Set
    End Property
    Public Property DamageMod() As Integer
        Get
            DamageMod = vs_DamageMod
        End Get
        Set(ByVal Value As Integer)
            vs_DamageMod = Value
        End Set
    End Property
    Public Property HealthMod() As Integer
        Get
            HealthMod = vs_HealthMod
        End Get
        Set(ByVal Value As Integer)
            vs_HealthMod = Value
        End Set
    End Property
    Public Property ExtraA9Flags() As Integer
        Get
            ExtraA9Flags = vs_ExtraA9Flags
        End Get
        Set(ByVal Value As Integer)
            vs_ExtraA9Flags = Value
        End Set
    End Property
    Public Property manatype() As Integer
        Get
            manatype = vs_manatype
        End Get
        Set(ByVal Value As Integer)
            vs_manatype = Value
        End Set
    End Property
    Public Property sell() As Integer
        Get
            sell = vs_sell
        End Get
        Set(ByVal Value As Integer)
            vs_sell = Value
        End Set
    End Property
    Public Property aiscript() As Integer
        Get
            aiscript = vs_aiscript
        End Get
        Set(ByVal Value As Integer)
            vs_aiscript = Value
        End Set
    End Property
    Public Property Resistance0_Armor() As Integer
        Get
            Resistance0_Armor = vs_Resistance0_Armor
        End Get
        Set(ByVal Value As Integer)
            vs_Resistance0_Armor = Value
        End Set
    End Property

#End Region
End Class
Class Spuriouscreatures
#Region "Variables"
    'Spurious Creatures table
    Dim vz_creature_id As Integer = 0
    Dim vz_creature_name As String = ""
    Dim vz_creature_guild As String = " "
    Dim vz_info_str As Integer = 0
    Dim vz_creature_model As Integer = 0
    Dim vz_creature_size As Integer = 0
    Dim vz_creature_life As Integer = 0
    Dim vz_creature_mana As Integer = 0
    Dim vz_creature_manaType As Integer = 0
    Dim vz_creature_elite As Integer = 0
    Dim vz_creature_faction As Integer = 0
    Dim vz_creature_family As Integer = 0
    Dim vz_creature_type As Integer = 0
    Dim vz_creature_minDamage As Integer = 0
    Dim vz_creature_maxDamage As Integer = 0
    Dim vz_creature_minRangedDamage As Integer = 0
    Dim vz_creature_maxRangedDamage As Integer = 0
    Dim vz_creature_attackPower As Integer = 0
    Dim vz_creature_rangedAttackPower As Integer = 0
    Dim vz_creature_armor As Integer = 0
    Dim vz_creature_resHoly As Integer = 0
    Dim vz_creature_resFire As Integer = 0
    Dim vz_creature_resNature As Integer = 0
    Dim vz_creature_resFrost As Integer = 0
    Dim vz_creature_resShadow As Integer = 0
    Dim vz_creature_resArcane As Integer = 0
    Dim vz_creature_walkSpeed As Integer = 0
    Dim vz_creature_runSpeed As Integer = 0
    Dim vz_creature_baseAttackSpeed As Integer = 0
    Dim vz_creature_baseRangedAttackSpeed As Integer = 0
    Dim vz_creature_combatReach As Integer = 0
    Dim vz_creature_bondingRadius As Integer = 0
    Dim vz_creature_npcflags As Integer = 0
    Dim vz_creature_flags As Integer = 0
    Dim vz_creature_minLevel As Integer = 0
    Dim vz_creature_maxLevel As Integer = 0
    Dim vz_creature_loot As Integer = 0
    Dim vz_creature_lootSkinning As Integer = 0
    Dim vz_creature_sell As Integer = 0
    Dim vz_creature_aiScript As Integer = 0
    'End of Spurious creatures table
#End Region
#Region "Properties"
    Public Property creature_id() As Integer
        Get
            creature_id = vz_creature_id
        End Get
        Set(ByVal Value As Integer)
            vz_creature_id = Value
        End Set
    End Property
    Public Property creature_name() As String
        Get
            creature_name = vz_creature_name
        End Get
        Set(ByVal Value As String)
            vz_creature_name = Value
        End Set
    End Property
    Public Property creature_guild() As String
        Get
            creature_guild = vz_creature_guild
        End Get
        Set(ByVal Value As String)
            vz_creature_guild = Value
        End Set
    End Property
    Public Property info_str() As Integer
        Get
            info_str = vz_info_str
        End Get
        Set(ByVal Value As Integer)
            vz_info_str = Value
        End Set
    End Property
    Public Property creature_model() As Integer
        Get
            creature_model = vz_creature_model
        End Get
        Set(ByVal Value As Integer)
            vz_creature_model = Value
        End Set
    End Property
    Public Property creature_size() As Integer
        Get
            creature_size = vz_creature_size
        End Get
        Set(ByVal Value As Integer)
            vz_creature_size = Value
        End Set
    End Property
    Public Property creature_life() As Integer
        Get
            creature_life = vz_creature_life
        End Get
        Set(ByVal Value As Integer)
            vz_creature_life = Value
        End Set
    End Property
    Public Property creature_mana() As Integer
        Get
            creature_mana = vz_creature_mana
        End Get
        Set(ByVal Value As Integer)
            vz_creature_mana = Value
        End Set
    End Property
    Public Property creature_manaType() As Integer
        Get
            creature_manaType = vz_creature_manaType
        End Get
        Set(ByVal Value As Integer)
            vz_creature_manaType = Value
        End Set
    End Property
    Public Property creature_elite() As Integer
        Get
            creature_elite = vz_creature_elite
        End Get
        Set(ByVal Value As Integer)
            vz_creature_elite = Value
        End Set
    End Property
    Public Property creature_faction() As Integer
        Get
            creature_faction = vz_creature_faction
        End Get
        Set(ByVal Value As Integer)
            vz_creature_faction = Value
        End Set
    End Property
    Public Property creature_family() As Integer
        Get
            creature_family = vz_creature_family
        End Get
        Set(ByVal Value As Integer)
            vz_creature_family = Value
        End Set
    End Property
    Public Property creature_type() As Integer
        Get
            creature_type = vz_creature_type
        End Get
        Set(ByVal Value As Integer)
            vz_creature_type = Value
        End Set
    End Property
    Public Property creature_minDamage() As Integer
        Get
            creature_minDamage = vz_creature_minDamage
        End Get
        Set(ByVal Value As Integer)
            vz_creature_minDamage = Value
        End Set
    End Property
    Public Property creature_maxDamage() As Integer
        Get
            creature_maxDamage = vz_creature_maxDamage
        End Get
        Set(ByVal Value As Integer)
            vz_creature_maxDamage = Value
        End Set
    End Property
    Public Property creature_minRangedDamage() As Integer
        Get
            creature_minRangedDamage = vz_creature_minRangedDamage
        End Get
        Set(ByVal Value As Integer)
            vz_creature_minRangedDamage = Value
        End Set
    End Property
    Public Property creature_maxRangedDamage() As Integer
        Get
            creature_maxRangedDamage = vz_creature_maxRangedDamage
        End Get
        Set(ByVal Value As Integer)
            vz_creature_maxRangedDamage = Value
        End Set
    End Property
    Public Property creature_attackPower() As Integer
        Get
            creature_attackPower = vz_creature_attackPower
        End Get
        Set(ByVal Value As Integer)
            vz_creature_attackPower = Value
        End Set
    End Property
    Public Property creature_rangedAttackPower() As Integer
        Get
            creature_rangedAttackPower = vz_creature_rangedAttackPower
        End Get
        Set(ByVal Value As Integer)
            vz_creature_rangedAttackPower = Value
        End Set
    End Property
    Public Property creature_armor() As Integer
        Get
            creature_armor = vz_creature_armor
        End Get
        Set(ByVal Value As Integer)
            vz_creature_armor = Value
        End Set
    End Property
    Public Property creature_resHoly() As Integer
        Get
            creature_resHoly = vz_creature_resHoly
        End Get
        Set(ByVal Value As Integer)
            vz_creature_resHoly = Value
        End Set
    End Property
    Public Property creature_resFire() As Integer
        Get
            creature_resFire = vz_creature_resFire
        End Get
        Set(ByVal Value As Integer)
            vz_creature_resFire = Value
        End Set
    End Property
    Public Property creature_resNature() As Integer
        Get
            creature_resNature = vz_creature_resNature
        End Get
        Set(ByVal Value As Integer)
            vz_creature_resNature = Value
        End Set
    End Property
    Public Property creature_resFrost() As Integer
        Get
            creature_resFrost = vz_creature_resFrost
        End Get
        Set(ByVal Value As Integer)
            vz_creature_resFrost = Value
        End Set
    End Property
    Public Property creature_resShadow() As Integer
        Get
            creature_resShadow = vz_creature_resShadow
        End Get
        Set(ByVal Value As Integer)
            vz_creature_resShadow = Value
        End Set
    End Property
    Public Property creature_walkSpeed() As Integer
        Get
            creature_walkSpeed = vz_creature_walkSpeed
        End Get
        Set(ByVal Value As Integer)
            vz_creature_walkSpeed = Value
        End Set
    End Property
    Public Property creature_runSpeed() As Integer
        Get
            creature_runSpeed = vz_creature_runSpeed
        End Get
        Set(ByVal Value As Integer)
            vz_creature_runSpeed = Value
        End Set
    End Property
    Public Property creature_baseAttackSpeed() As Integer
        Get
            creature_baseAttackSpeed = vz_creature_baseAttackSpeed
        End Get
        Set(ByVal Value As Integer)
            vz_creature_baseAttackSpeed = Value
        End Set
    End Property
    Public Property creature_baseRangedAttackSpeed() As Integer
        Get
            creature_baseRangedAttackSpeed = vz_creature_baseRangedAttackSpeed
        End Get
        Set(ByVal Value As Integer)
            vz_creature_baseRangedAttackSpeed = Value
        End Set
    End Property
    Public Property creature_combatReach() As Integer
        Get
            creature_combatReach = vz_creature_combatReach
        End Get
        Set(ByVal Value As Integer)
            vz_creature_combatReach = Value
        End Set
    End Property
    Public Property creature_bondingRadius() As Integer
        Get
            creature_bondingRadius = vz_creature_bondingRadius
        End Get
        Set(ByVal Value As Integer)
            vz_creature_bondingRadius = Value
        End Set
    End Property
    Public Property creature_npcflags() As Integer
        Get
            creature_npcflags = vz_creature_npcflags
        End Get
        Set(ByVal Value As Integer)
            vz_creature_npcflags = Value
        End Set
    End Property
    Public Property creature_flags() As Integer
        Get
            creature_flags = vz_creature_flags
        End Get
        Set(ByVal Value As Integer)
            vz_creature_flags = Value
        End Set
    End Property
    Public Property creature_minLevel() As Integer
        Get
            creature_minLevel = vz_creature_minLevel
        End Get
        Set(ByVal Value As Integer)
            vz_creature_minLevel = Value
        End Set
    End Property
    Public Property creature_maxLevel() As Integer
        Get
            creature_maxLevel = vz_creature_maxLevel
        End Get
        Set(ByVal Value As Integer)
            vz_creature_maxLevel = Value
        End Set
    End Property
    Public Property creature_loot() As Integer
        Get
            creature_loot = vz_creature_loot
        End Get
        Set(ByVal Value As Integer)
            vz_creature_loot = Value
        End Set
    End Property
    Public Property creature_lootSkinning() As Integer
        Get
            creature_lootSkinning = vz_creature_lootSkinning
        End Get
        Set(ByVal Value As Integer)
            vz_creature_lootSkinning = Value
        End Set
    End Property
    Public Property creature_sell() As Integer
        Get
            creature_sell = vz_creature_sell
        End Get
        Set(ByVal Value As Integer)
            vz_creature_sell = Value
        End Set
    End Property
    Public Property creature_aiScript() As Integer
        Get
            creature_aiScript = vz_creature_aiScript
        End Get
        Set(ByVal Value As Integer)
            vz_creature_aiScript = Value
        End Set
    End Property
#End Region
End Class
