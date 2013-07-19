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
Public Class AscentCreatures

#Region "Constants"
    Public v_entry As Integer = 0
    Public v_creature_name As String = " "
    Public v_subname As String = " "
    Public v_info_str As String = " "
    Public v_flags1 As Integer = 0
    Public v_type As Integer = 0
    Public v_family As Integer = 0
    Public v_rank As Integer = 0
    Public v_unk4 As Integer = 0
    Public v_spelldataid As Integer = 0
    Public v_male_displayid As Integer = 0
    Public v_female_displayid As Integer = 0
    Public v_male_displayid2 As Integer = 0
    Public v_female_displayid2 As Integer = 0
    Public v_unknown_float1 As Single = 0
    Public v_unknown_float2 As Single = 0
    Public v_civilian As Integer = 0
    Public v_leader As Integer = 0
    Public v_minlevel As Integer = 0
    Public v_maxlevel As Integer = 0
    Public v_faction As Integer = 0
    Public v_minhealth As Integer = 0
    Public v_maxhealth As Integer = 0
    Public v_mana As Integer = 0
    Public v_scale As Single = 0
    Public v_npcflags As Integer = 0
    Public v_attacktime As Integer = 0
    Public v_attackpower As Integer = 0
    Public v_rangedattackpower As Integer = 0
    Public v_attacktype As Integer = 0
    Public v_mindamage As Single = 0
    Public v_maxdamage As Single = 0
    Public v_rangedattacktime As Integer = 0
    Public v_rangedmindamage As Single = 0
    Public v_rangedmaxdamage As Single = 0
    Public v_mountdisplayid As Integer = 0
    Public v_equipmententry As Integer = 0
    Public v_equipmodel1 As Integer = 0
    Public v_equipinfo1 As Integer = 0
    Public v_equipslot1 As Integer = 0
    Public v_equipmodel2 As Integer = 0
    Public v_equipinfo2 As Integer = 0
    Public v_equipslot2 As Integer = 0
    Public v_equipmodel3 As Integer = 0
    Public v_equipinfo3 As Integer = 0
    Public v_equipslot3 As Integer = 0
    Public v_respawntime As Integer = 0
    Public v_resistance0_armor As Integer = 0
    Public v_resistance1 As Integer = 0
    Public v_resistance2 As Integer = 0
    Public v_resistance3 As Integer = 0
    Public v_resistance4 As Integer = 0
    Public v_resistance5 As Integer = 0
    Public v_resistance6 As Integer = 0
    Public v_combat_reach As Single = 0
    Public v_bounding_radius As Single = 0
    Public v_auras As String = " "
    Public v_boss As Integer = 0
    Public v_money As Integer = 0
    Public v_invisibility_type As Integer = 0
    Public v_death_state As Integer = 0
    Public v_dynamicflags As Integer = 0
    Public v_flagsextra As Integer = 0
    Public v_movetype As Integer = 0
    Public v_walk_speed As Single = 0
    Public v_run_speed As Single = 0
    Public v_fly_speed As Single = 0
    Public v_lootid As Integer = 0
    Public v_skinloot As Integer = 0
    Public v_pickpocketloot As Integer = 0
    Public v_inhabittype As Integer = 0
    Public v_hasregan As Integer = 0
    Public v_respawnmod As Single = 0
    Public v_armormod As Single = 0
    Public v_damagemod As Single = 0
    Public v_healthmod As Single = 0
    Public v_extra_a9_flags As Integer = 0
    Public sv_maxhealth As Integer = 0
    Public sv_maxmana As Integer = 0
    Public sv_armor As Integer = 0
    Public sv_level As Integer = 0
    Public sv_faction As Integer = 0
    Public sv_npcflag As Integer = 0
    Public sv_scale As Single = 0
    Public sv_speed As Single = 0
    Public sv_mindmg As Single = 0
    Public sv_maxdmg As Single = 0
    Public sv_minrangedmg As Single = 0
    Public sv_maxrangedmg As Single = 0
    Public sv_baseattacktime As Integer = 0
    Public sv_rangeattacktime As Integer = 0
    Public sv_boundingradius As Single = 0
    Public sv_combatreach As Single = 0
    Public sv_slot1model As Integer = 0
    Public sv_slot1info1 As Integer = 0
    Public sv_slot1info2 As Integer = 0
    Public sv_slot1info3 As Integer = 0
    Public sv_slot1info4 As Integer = 0
    Public sv_slot1info5 As Integer = 0
    Public sv_slot2model As Integer = 0
    Public sv_slot2info1 As Integer = 0
    Public sv_slot2info2 As Integer = 0
    Public sv_slot2info3 As Integer = 0
    Public sv_slot2info4 As Integer = 0
    Public sv_slot2info5 As Integer = 0
    Public sv_slot3model As Integer = 0
    Public sv_slot3info1 As Integer = 0
    Public sv_slot3info2 As Integer = 0
    Public sv_slot3info3 As Integer = 0
    Public sv_slot3info4 As Integer = 0
    Public sv_slot3info5 As Integer = 0
    Public sv_mount As Integer = 0
    Public as_map As Integer = 0
    Public as_position_x As Single = 0
    Public as_position_y As Single = 0
    Public as_position_z As Single = 0
    Public as_orientation As Single = 0
    Public as_movetype As Integer = 0
    Public as_displayid As Integer = 0
    Public as_factionid As Integer = 0
    Public as_flags As Integer = 0
    Public as_bytes As Integer = 0
    Public as_bytes2 As Integer = 0
    Public as_emotestate As Integer = 0
    Public as_respawnnpclink As Integer = 0
    Public as_channel_spell As Integer = 0
    Public as_channel_target_sqlid As Integer = 0
    Public as_channel_target_sqlid_creature As Integer = 0
    Public as_standstate As Integer = 0
    Public singlequote As String = Chr(34)
    Public doublequote As String = Chr(34) & Chr(34)

#End Region

#Region "Properties"
    Public Property Entry() As Integer
        Get
            Entry = v_entry
        End Get
        Set(ByVal Value As Integer)
            v_entry = Value
        End Set
    End Property

    Public Property Creature_Name() As String
        Get
            Creature_Name = v_creature_name
        End Get
        Set(ByVal Value As String)
            Value = Replace(Value, singlequote, doublequote)
            v_creature_name = Value
        End Set
    End Property

    Public Property Subname() As String
        Get
            Subname = v_subname
        End Get
        Set(ByVal Value As String)
            Value = Replace(Value, singlequote, doublequote)
            v_subname = Value
        End Set
    End Property

    Public Property InfoStr() As String
        Get
            InfoStr = v_info_str
        End Get
        Set(ByVal Value As String)
            v_info_str = Value
        End Set
    End Property

    Public Property Flags1() As Integer
        Get
            Flags1 = v_flags1
        End Get
        Set(ByVal Value As Integer)
            v_flags1 = Value
        End Set
    End Property

    Public Property Type() As Integer
        Get
            Type = v_type
        End Get
        Set(ByVal Value As Integer)
            v_type = Value
        End Set
    End Property

    Public Property Family() As Integer
        Get
            Family = v_family
        End Get
        Set(ByVal Value As Integer)
            v_family = Value
        End Set
    End Property

    Public Property Rank() As Integer
        Get
            Rank = v_rank
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

    Public Property SpellDataID() As Integer
        Get
            SpellDataID = v_spelldataid
        End Get
        Set(ByVal Value As Integer)
            v_spelldataid = Value
        End Set
    End Property

    Public Property DisplayID() As Integer
        Get
            DisplayID = v_male_displayid
        End Get
        Set(ByVal Value As Integer)
            v_male_displayid = Value
        End Set
    End Property

    Public Property Female_DisplayID() As Integer
        Get
            Female_DisplayID = v_female_displayid
        End Get
        Set(ByVal Value As Integer)
            v_female_displayid = Value
        End Set
    End Property

    Public Property Male_DisplayID2() As Integer
        Get
            Male_DisplayID2 = v_male_displayid2
        End Get
        Set(ByVal Value As Integer)
            v_male_displayid2 = Value
        End Set
    End Property

    Public Property Female_DisplayID2() As Integer
        Get
            Female_DisplayID2 = v_female_displayid2
        End Get
        Set(ByVal Value As Integer)
            v_female_displayid2 = Value
        End Set
    End Property

    Public Property Unknown_Float1() As Single
        Get
            Unknown_Float1 = v_unknown_float1
        End Get
        Set(ByVal Value As Single)
            v_unknown_float1 = Value
        End Set
    End Property

    Public Property Unknown_Float2() As Single
        Get
            Unknown_Float2 = v_unknown_float2
        End Get
        Set(ByVal Value As Single)
            v_unknown_float2 = Value
        End Set
    End Property

    Public Property Civilian() As Integer
        Get
            Civilian = v_civilian
        End Get
        Set(ByVal Value As Integer)
            v_civilian = Value
        End Set
    End Property

    Public Property Leader() As Integer
        Get
            Leader = v_leader
        End Get
        Set(ByVal Value As Integer)
            v_leader = Value
        End Set
    End Property

    Public Property MinLevel() As Integer
        Get
            MinLevel = v_minlevel
        End Get
        Set(ByVal Value As Integer)
            v_minlevel = Value
        End Set
    End Property

    Public Property MaxLevel() As Integer
        Get
            MaxLevel = v_maxlevel
        End Get
        Set(ByVal Value As Integer)
            v_maxlevel = Value
        End Set
    End Property

    Public Property Faction() As Integer
        Get
            Faction = v_faction
        End Get
        Set(ByVal Value As Integer)
            v_faction = Value
        End Set
    End Property

    Public Property MinHealth() As Integer
        Get
            MinHealth = v_minhealth
        End Get
        Set(ByVal Value As Integer)
            v_minhealth = Value
        End Set
    End Property

    Public Property MaxHealth() As Integer
        Get
            MaxHealth = v_maxhealth
        End Get
        Set(ByVal Value As Integer)
            v_maxhealth = Value
        End Set
    End Property

    Public Property Mana() As Integer
        Get
            Mana = v_mana
        End Get
        Set(ByVal Value As Integer)
            v_mana = Value
        End Set
    End Property

    Public Property Scale() As Single
        Get
            Scale = v_scale
        End Get
        Set(ByVal Value As Single)
            v_scale = Value
        End Set
    End Property

    Public Property npcFlags() As Integer
        Get
            npcFlags = v_npcflags
        End Get
        Set(ByVal Value As Integer)
            v_npcflags = Value
        End Set
    End Property

    Public Property AttackTime() As Integer
        Get
            AttackTime = v_attacktime
        End Get
        Set(ByVal Value As Integer)
            v_attacktime = Value
        End Set
    End Property

    Public Property AttackPower() As Integer
        Get
            AttackPower = v_attackpower
        End Get
        Set(ByVal Value As Integer)
            v_attackpower = Value
        End Set
    End Property

    Public Property RangedAttackPower() As Integer
        Get
            RangedAttackPower = v_rangedattackpower
        End Get
        Set(ByVal Value As Integer)
            v_rangedattackpower = Value
        End Set
    End Property

    Public Property AttackType() As Integer
        Get
            AttackType = v_attacktype
        End Get
        Set(ByVal Value As Integer)
            v_attacktype = Value
        End Set
    End Property

    Public Property MinDamage() As Single
        Get
            MinDamage = v_mindamage
        End Get
        Set(ByVal Value As Single)
            v_mindamage = Value
        End Set
    End Property

    Public Property MaxDamage() As Single
        Get
            MaxDamage = v_maxdamage
        End Get
        Set(ByVal Value As Single)
            v_maxdamage = Value
        End Set
    End Property

    Public Property RangedAttackTime() As Integer
        Get
            RangedAttackTime = v_rangedattacktime
        End Get
        Set(ByVal Value As Integer)
            v_rangedattacktime = Value
        End Set
    End Property

    Public Property RangedMinDamage() As Single
        Get
            RangedMinDamage = v_rangedmindamage
        End Get
        Set(ByVal Value As Single)
            v_rangedmindamage = Value
        End Set
    End Property

    Public Property RangedMaxDamage() As Single
        Get
            RangedMaxDamage = v_rangedmaxdamage
        End Get
        Set(ByVal Value As Single)
            v_rangedmaxdamage = Value
        End Set
    End Property

    Public Property MountDisplayID() As Integer
        Get
            MountDisplayID = v_mountdisplayid
        End Get
        Set(ByVal Value As Integer)
            v_mountdisplayid = Value
        End Set
    End Property

    Public Property EquipmentEntry() As Integer
        Get
            EquipmentEntry = v_equipmententry
        End Get
        Set(ByVal Value As Integer)
            v_equipmententry = Value
        End Set
    End Property

    Public Property EquipModel1() As Integer
        Get
            EquipModel1 = v_equipmodel1
        End Get
        Set(ByVal Value As Integer)
            v_equipmodel1 = Value
        End Set
    End Property

    Public Property EquipInfo1() As Integer
        Get
            EquipInfo1 = v_equipinfo1
        End Get
        Set(ByVal Value As Integer)
            v_equipinfo1 = Value
        End Set
    End Property

    Public Property EquipSlot1() As Integer
        Get
            EquipSlot1 = v_equipslot1
        End Get
        Set(ByVal Value As Integer)
            v_equipslot1 = Value
        End Set
    End Property

    Public Property EquipModel2() As Integer
        Get
            EquipModel2 = v_equipmodel2
        End Get
        Set(ByVal Value As Integer)
            v_equipmodel2 = Value
        End Set
    End Property

    Public Property EquipInfo2() As Integer
        Get
            EquipInfo2 = v_equipinfo2
        End Get
        Set(ByVal Value As Integer)
            v_equipinfo2 = Value
        End Set
    End Property

    Public Property EquipSlot2() As Integer
        Get
            EquipSlot2 = v_equipslot2
        End Get
        Set(ByVal Value As Integer)
            v_equipslot2 = Value
        End Set
    End Property

    Public Property EquipModel3() As Integer
        Get
            EquipModel3 = v_equipmodel3
        End Get
        Set(ByVal Value As Integer)
            v_equipmodel3 = Value
        End Set
    End Property

    Public Property EquipInfo3() As Integer
        Get
            EquipInfo3 = v_equipinfo3
        End Get
        Set(ByVal Value As Integer)
            v_equipinfo3 = Value
        End Set
    End Property

    Public Property EquipSlot3() As Integer
        Get
            EquipSlot3 = v_equipslot3
        End Get
        Set(ByVal Value As Integer)
            v_equipslot3 = Value
        End Set
    End Property

    Public Property WalkSpeed() As Single
        Get
            WalkSpeed = v_walk_speed
        End Get
        Set(ByVal Value As Single)
            v_walk_speed = Value
        End Set
    End Property

    Public Property RunSpeed() As Single
        Get
            RunSpeed = v_run_speed
        End Get
        Set(ByVal Value As Single)
            v_run_speed = Value
        End Set
    End Property

    Public Property FlySpeed() As Single
        Get
            FlySpeed = v_fly_speed
        End Get
        Set(ByVal Value As Single)
            v_fly_speed = Value
        End Set
    End Property

    Public Property LootID() As Integer
        Get
            LootID = v_lootid
        End Get
        Set(ByVal Value As Integer)
            v_lootid = Value
        End Set
    End Property

    Public Property SkinLoot() As Integer
        Get
            SkinLoot = v_skinloot
        End Get
        Set(ByVal Value As Integer)
            v_skinloot = Value
        End Set
    End Property

    Public Property PickPocketLoot() As Integer
        Get
            PickPocketLoot = v_pickpocketloot
        End Get
        Set(ByVal Value As Integer)
            v_pickpocketloot = Value
        End Set
    End Property

    Public Property InhabitType() As Integer
        Get
            InhabitType = v_inhabittype
        End Get
        Set(ByVal Value As Integer)
            v_inhabittype = Value
        End Set
    End Property

    Public Property HasRegan() As Integer
        Get
            HasRegan = v_hasregan
        End Get
        Set(ByVal Value As Integer)
            v_hasregan = Value
        End Set
    End Property

    Public Property RespawnMod() As Single
        Get
            RespawnMod = v_respawnmod
        End Get
        Set(ByVal Value As Single)
            v_respawnmod = Value
        End Set
    End Property

    Public Property ArmorMod() As Single
        Get
            ArmorMod = v_armormod
        End Get
        Set(ByVal Value As Single)
            v_armormod = Value
        End Set
    End Property

    Public Property DamageMod() As Single
        Get
            DamageMod = v_damagemod
        End Get
        Set(ByVal Value As Single)
            v_damagemod = Value
        End Set
    End Property

    Public Property HealthMod() As Single
        Get
            HealthMod = v_healthmod
        End Get
        Set(ByVal Value As Single)
            v_healthmod = Value
        End Set
    End Property

    Public Property ExtraA9Flags() As Integer
        Get
            ExtraA9Flags = v_extra_a9_flags
        End Get
        Set(ByVal Value As Integer)
            v_extra_a9_flags = Value
        End Set
    End Property

    Public Property RespawnTime() As Integer
        Get
            RespawnTime = v_respawntime
        End Get
        Set(ByVal Value As Integer)
            If Value > 0 Then
                v_respawntime = (Value / 60)
            Else
                v_respawntime = Value
            End If
        End Set
    End Property

    Public Property Resistance0_Armor() As Integer
        Get
            Resistance0_Armor = v_resistance0_armor
        End Get
        Set(ByVal Value As Integer)
            v_resistance0_armor = Value
        End Set
    End Property

    Public Property Resistance1() As Integer
        Get
            Resistance1 = v_resistance1
        End Get
        Set(ByVal Value As Integer)
            v_resistance1 = Value
        End Set
    End Property

    Public Property Resistance2() As Integer
        Get
            Resistance2 = v_resistance2
        End Get
        Set(ByVal Value As Integer)
            v_resistance2 = Value
        End Set
    End Property

    Public Property Resistance3() As Integer
        Get
            Resistance3 = v_resistance3
        End Get
        Set(ByVal Value As Integer)
            v_resistance3 = Value
        End Set
    End Property

    Public Property Resistance4() As Integer
        Get
            Resistance4 = v_resistance4
        End Get
        Set(ByVal Value As Integer)
            v_resistance4 = Value
        End Set
    End Property

    Public Property Resistance5() As Integer
        Get
            Resistance5 = v_resistance5
        End Get
        Set(ByVal Value As Integer)
            v_resistance5 = Value
        End Set
    End Property

    Public Property Resistance6() As Integer
        Get
            Resistance6 = v_resistance6
        End Get
        Set(ByVal Value As Integer)
            v_resistance6 = Value
        End Set
    End Property

    Public Property Combat_Reach() As Single
        Get
            Combat_Reach = v_combat_reach
        End Get
        Set(ByVal Value As Single)
            v_combat_reach = Value
        End Set
    End Property

    Public Property Bounding_Radius() As Single
        Get
            Bounding_Radius = v_bounding_radius
        End Get
        Set(ByVal Value As Single)
            v_bounding_radius = Value
        End Set
    End Property

    Public Property Auras() As String
        Get
            Auras = v_auras
        End Get
        Set(ByVal Value As String)
            v_auras = Value
        End Set
    End Property

    Public Property Boss() As Integer
        Get
            Boss = v_boss
        End Get
        Set(ByVal Value As Integer)
            v_boss = Value
        End Set
    End Property

    Public Property Money() As Integer
        Get
            Money = v_money
        End Get
        Set(ByVal Value As Integer)
            v_money = Value
        End Set
    End Property

    Public Property Invisibility_Type() As Integer
        Get
            Invisibility_Type = v_invisibility_type
        End Get
        Set(ByVal Value As Integer)
            v_invisibility_type = Value
        End Set
    End Property

    Public Property Death_State() As Integer
        Get
            Death_State = v_death_state
        End Get
        Set(ByVal Value As Integer)
            v_death_state = Value
        End Set
    End Property

    Public Property DynamicFlags() As Integer
        Get
            DynamicFlags = v_dynamicflags
        End Get
        Set(ByVal Value As Integer)
            v_dynamicflags = Value
        End Set
    End Property

    Public Property FlagsExtra() As Integer
        Get
            FlagsExtra = v_flagsextra
        End Get
        Set(ByVal Value As Integer)
            v_flagsextra = Value
        End Set
    End Property

    Public Property MoveType() As Integer
        Get
            MoveType = v_movetype
        End Get
        Set(ByVal Value As Integer)
            v_movetype = Value
        End Set
    End Property

    Public Property Spawn_MaxHealth() As Integer
        Get
            Spawn_MaxHealth = sv_maxhealth
        End Get
        Set(ByVal Value As Integer)
            sv_maxhealth = Value
        End Set
    End Property

    Public Property Spawn_MaxMana() As Integer
        Get
            Spawn_MaxMana = sv_maxmana
        End Get
        Set(ByVal Value As Integer)
            sv_maxmana = Value
        End Set
    End Property

    Public Property Spawn_Armor() As Integer
        Get
            Spawn_Armor = sv_armor
        End Get
        Set(ByVal Value As Integer)
            sv_armor = Value
        End Set
    End Property

    Public Property Spawn_Level() As Integer
        Get
            Spawn_Level = sv_level
        End Get
        Set(ByVal Value As Integer)
            sv_level = Value
        End Set
    End Property

    Public Property Spawn_Faction() As Integer
        Get
            Spawn_Faction = sv_faction
        End Get
        Set(ByVal Value As Integer)
            sv_faction = Value
        End Set
    End Property

    Public Property Spawn_npcFlag() As Integer
        Get
            Spawn_npcFlag = sv_npcflag
        End Get
        Set(ByVal Value As Integer)
            sv_npcflag = Value
        End Set
    End Property

    Public Property Spawn_Scale() As Single
        Get
            Spawn_Scale = sv_scale
        End Get
        Set(ByVal Value As Single)
            sv_scale = Value
        End Set
    End Property

    Public Property Spawn_Speed() As Single
        Get
            Spawn_Speed = sv_speed
        End Get
        Set(ByVal Value As Single)
            sv_speed = Value
        End Set
    End Property

    Public Property Spawn_MinDamage() As Single
        Get
            Spawn_MinDamage = sv_mindmg
        End Get
        Set(ByVal Value As Single)
            sv_mindmg = Value
        End Set
    End Property

    Public Property Spawn_MaxDamage() As Single
        Get
            Spawn_MaxDamage = sv_maxdmg
        End Get
        Set(ByVal Value As Single)
            sv_maxdmg = Value
        End Set
    End Property

    Public Property Spawn_MinRangedDamage() As Single
        Get
            Spawn_MinRangedDamage = sv_minrangedmg
        End Get
        Set(ByVal Value As Single)
            sv_minrangedmg = Value
        End Set
    End Property

    Public Property Spawn_MaxRangedDamage() As Single
        Get
            Spawn_MaxRangedDamage = sv_maxrangedmg
        End Get
        Set(ByVal Value As Single)
            sv_maxrangedmg = Value
        End Set
    End Property

    Public Property Spawn_BaseAttackTime() As Integer
        Get
            Spawn_BaseAttackTime = sv_baseattacktime
        End Get
        Set(ByVal Value As Integer)
            sv_baseattacktime = Value
        End Set
    End Property

    Public Property Spawn_RangeAttackTime() As Integer
        Get
            Spawn_RangeAttackTime = sv_rangeattacktime
        End Get
        Set(ByVal Value As Integer)
            sv_rangeattacktime = Value
        End Set
    End Property

    Public Property Spawn_BoundingRadius() As Single
        Get
            Spawn_BoundingRadius = sv_boundingradius
        End Get
        Set(ByVal Value As Single)
            sv_boundingradius = Value
        End Set
    End Property

    Public Property Spawn_CombatReach() As Single
        Get
            Spawn_CombatReach = sv_combatreach
        End Get
        Set(ByVal Value As Single)
            sv_combatreach = Value
        End Set
    End Property

    Public Property Spawn_Slot1Model() As Integer
        Get
            Spawn_Slot1Model = sv_slot1model
        End Get
        Set(ByVal Value As Integer)
            sv_slot1model = Value
        End Set
    End Property

    Public Property Spawn_Slot1Info1() As Integer
        Get
            Spawn_Slot1Info1 = sv_slot1info1
        End Get
        Set(ByVal Value As Integer)
            sv_slot1info1 = Value
        End Set
    End Property

    Public Property Spawn_Slot1Info2() As Integer
        Get
            Spawn_Slot1Info2 = sv_slot1info2
        End Get
        Set(ByVal Value As Integer)
            sv_slot1info2 = Value
        End Set
    End Property

    Public Property Spawn_Slot1Info3() As Integer
        Get
            Spawn_Slot1Info3 = sv_slot1info3
        End Get
        Set(ByVal Value As Integer)
            sv_slot1info3 = Value
        End Set
    End Property

    Public Property Spawn_Slot1Info4() As Integer
        Get
            Spawn_Slot1Info4 = sv_slot1info4
        End Get
        Set(ByVal Value As Integer)
            sv_slot1info4 = Value
        End Set
    End Property

    Public Property Spawn_Slot1Info5() As Integer
        Get
            Spawn_Slot1Info5 = sv_slot1info5
        End Get
        Set(ByVal Value As Integer)
            sv_slot1info5 = Value
        End Set
    End Property

    Public Property Spawn_Slot2Model() As Integer
        Get
            Spawn_Slot2Model = sv_slot2model
        End Get
        Set(ByVal Value As Integer)
            sv_slot2model = Value
        End Set
    End Property

    Public Property Spawn_Slot2Info1() As Integer
        Get
            Spawn_Slot2Info1 = sv_slot2info1
        End Get
        Set(ByVal Value As Integer)
            sv_slot2info1 = Value
        End Set
    End Property

    Public Property Spawn_Slot2Info2() As Integer
        Get
            Spawn_Slot2Info2 = sv_slot2info2
        End Get
        Set(ByVal Value As Integer)
            sv_slot2info2 = Value
        End Set
    End Property

    Public Property Spawn_Slot2Info3() As Integer
        Get
            Spawn_Slot2Info3 = sv_slot2info3
        End Get
        Set(ByVal Value As Integer)
            sv_slot2info3 = Value
        End Set
    End Property

    Public Property Spawn_Slot2Info4() As Integer
        Get
            Spawn_Slot2Info4 = sv_slot2info4
        End Get
        Set(ByVal Value As Integer)
            sv_slot2info4 = Value
        End Set
    End Property

    Public Property Spawn_Slot2Info5() As Integer
        Get
            Spawn_Slot2Info5 = sv_slot2info5
        End Get
        Set(ByVal Value As Integer)
            sv_slot2info5 = Value
        End Set
    End Property

    Public Property Spawn_Slot3Model() As Integer
        Get
            Spawn_Slot3Model = sv_slot3model
        End Get
        Set(ByVal Value As Integer)
            sv_slot3model = Value
        End Set
    End Property

    Public Property Spawn_Slot3Info1() As Integer
        Get
            Spawn_Slot3Info1 = sv_slot3info1
        End Get
        Set(ByVal Value As Integer)
            sv_slot3info1 = Value
        End Set
    End Property

    Public Property Spawn_Slot3Info2() As Integer
        Get
            Spawn_Slot3Info2 = sv_slot3info2
        End Get
        Set(ByVal Value As Integer)
            sv_slot3info2 = Value
        End Set
    End Property

    Public Property Spawn_Slot3Info3() As Integer
        Get
            Spawn_Slot3Info3 = sv_slot3info3
        End Get
        Set(ByVal Value As Integer)
            sv_slot3info3 = Value
        End Set
    End Property

    Public Property Spawn_Slot3Info4() As Integer
        Get
            Spawn_Slot3Info4 = sv_slot3info4
        End Get
        Set(ByVal Value As Integer)
            sv_slot3info4 = Value
        End Set
    End Property

    Public Property Spawn_Slot3Info5() As Integer
        Get
            Spawn_Slot3Info5 = sv_slot3info5
        End Get
        Set(ByVal Value As Integer)
            sv_slot3info5 = Value
        End Set
    End Property

    Public Property Spawn_Mount() As Integer
        Get
            Spawn_Mount = sv_mount
        End Get
        Set(ByVal Value As Integer)
            sv_mount = Value
        End Set
    End Property

    Public Property ASpawn_Map() As Single
        Get
            ASpawn_Map = as_map
        End Get
        Set(ByVal Value As Single)
            as_map = Value
        End Set
    End Property

    Public Property ASpawn_X() As Single
        Get
            ASpawn_X = as_position_x
        End Get
        Set(ByVal Value As Single)
            as_position_x = Value
        End Set
    End Property

    Public Property ASpawn_Y() As Single
        Get
            ASpawn_Y = as_position_y
        End Get
        Set(ByVal Value As Single)
            as_position_y = Value
        End Set
    End Property

    Public Property ASpawn_Z() As Single
        Get
            ASpawn_Z = as_position_z
        End Get
        Set(ByVal Value As Single)
            as_position_z = Value
        End Set
    End Property

    Public Property ASpawn_O() As Single
        Get
            ASpawn_O = as_orientation
        End Get
        Set(ByVal Value As Single)
            as_orientation = Value
        End Set
    End Property

    Public Property ASpawn_MoveType() As Integer
        Get
            ASpawn_MoveType = as_movetype
        End Get
        Set(ByVal Value As Integer)
            as_movetype = Value
        End Set
    End Property

    Public Property ASpawn_DisplayID() As Integer
        Get
            ASpawn_DisplayID = as_displayid
        End Get
        Set(ByVal Value As Integer)
            as_displayid = Value
        End Set
    End Property

    Public Property ASpawn_FactionID() As Integer
        Get
            ASpawn_FactionID = as_factionid
        End Get
        Set(ByVal Value As Integer)
            as_factionid = Value
        End Set
    End Property

    Public Property ASpawn_Flags() As Integer
        Get
            ASpawn_Flags = as_flags
        End Get
        Set(ByVal Value As Integer)
            as_flags = Value
        End Set
    End Property

    Public Property ASpawn_Bytes() As Integer
        Get
            ASpawn_Bytes = as_bytes
        End Get
        Set(ByVal Value As Integer)
            as_bytes = Value
        End Set
    End Property

    Public Property ASpawn_Bytes2() As Integer
        Get
            ASpawn_Bytes2 = as_bytes2
        End Get
        Set(ByVal Value As Integer)
            as_bytes2 = Value
        End Set
    End Property

    Public Property ASpawn_Emote_State() As Integer
        Get
            ASpawn_Emote_State = as_emotestate
        End Get
        Set(ByVal Value As Integer)
            as_emotestate = Value
        End Set
    End Property

    Public Property ASpawn_RespawnNpcLink() As Integer
        Get
            ASpawn_RespawnNpcLink = as_respawnnpclink
        End Get
        Set(ByVal Value As Integer)
            as_respawnnpclink = Value
        End Set
    End Property

    Public Property ASpawn_ChannelSpell() As Integer
        Get
            ASpawn_ChannelSpell = as_channel_spell
        End Get
        Set(ByVal Value As Integer)
            as_channel_spell = Value
        End Set
    End Property

    Public Property ASpawn_ChannelTargetSQLID() As Integer
        Get
            ASpawn_ChannelTargetSQLID = as_channel_target_sqlid
        End Get
        Set(ByVal Value As Integer)
            as_channel_target_sqlid = Value
        End Set
    End Property

    Public Property ASpawn_ChannelTargetSQLIDCreature() As Integer
        Get
            ASpawn_ChannelTargetSQLIDCreature = as_channel_target_sqlid_creature
        End Get
        Set(ByVal Value As Integer)
            as_channel_target_sqlid_creature = Value
        End Set
    End Property

    Public Property ASpawn_StandState() As Integer
        Get
            ASpawn_StandState = as_standstate
        End Get
        Set(ByVal Value As Integer)
            as_standstate = Value
        End Set
    End Property

#End Region

End Class
