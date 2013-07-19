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

Imports System.Threading
Imports mangosVB.Common.BaseWriter

Public Module WS_Spells
#Region "WS.Skills.Constants"
    Public Enum TradeSkill As Integer
        TRADESKILL_ALCHEMY = 1
        TRADESKILL_BLACKSMITHING = 2
        TRADESKILL_COOKING = 3
        TRADESKILL_ENCHANTING = 4
        TRADESKILL_ENGINEERING = 5
        TRADESKILL_FIRSTAID = 6
        TRADESKILL_HERBALISM = 7
        TRADESKILL_LEATHERWORKING = 8
        TRADESKILL_POISONS = 9
        TRADESKILL_TAILORING = 10
        TRADESKILL_MINING = 11
        TRADESKILL_FISHING = 12
        TRADESKILL_SKINNING = 13
    End Enum
    Public Enum TradeSkillLevel As Integer
        TRADESKILL_LEVEL_NONE = 0
        TRADESKILL_LEVEL_APPRENTICE = 1
        TRADESKILL_LEVEL_JOURNEYMAN = 2
        TRADESKILL_LEVEL_EXPERT = 3
        TRADESKILL_LEVEL_ARTISAN = 4
        TRADESKILL_LEVEL_MASTER = 5
    End Enum
#End Region
#Region "WS.Spells.Constants"
    Public Const SPELL_DURATION_INFINITE As Integer = -1
    Public Const MAX_AURA_EFFECTs_VISIBLE As Integer = 56                  '56 AuraSlots (40 buff, 16 debuff)
    Public Const MAX_AURA_EFFECTs_PASSIVE As Integer = 192
    Public Const MAX_AURA_EFFECTs As Integer = MAX_AURA_EFFECTs_VISIBLE + MAX_AURA_EFFECTs_PASSIVE
    Public Const MAX_AURA_EFFECT_FLAGs As Integer = MAX_AURA_EFFECTs_VISIBLE \ 4
    Public Const MAX_POSITIVE_AURA_EFFECTs As Integer = 40
    Public Const MAX_NEGATIVE_AURA_EFFECTs As Integer = MAX_AURA_EFFECTs_VISIBLE - MAX_POSITIVE_AURA_EFFECTs


    Public Enum TargetType
        AllCharacters = -2
        AllMobiles = -3
        Enemy = -1
        [Friend] = 1
        GameObj = 4
        Neutral = 0
        Party = 2
        Pet = 3
    End Enum
    Public Enum TrackableCreatures
        All = 128
        Beast = 1
        Critter = 8
        Demon = 3
        Dragonkin = 2
        Elemental = 4
        Giant = 5
        Humanoid = 7
        Mechanical = 9
        Undead = 6
    End Enum
    Public Enum TrackableResources
        ElvenGems = 7
        GahzRidian = 15
        Herbs = 2
        Minerals = 3
        Treasure = 6
    End Enum

    Public Enum SpellCastState As Byte
        SPELL_STATE_NULL = 0
        SPELL_STATE_PREPARING = 1
        SPELL_STATE_CASTING = 2
        SPELL_STATE_FINISHED = 3
        SPELL_STATE_IDLE = 4
    End Enum
    Public Enum SpellSchoolMask As Byte
        SPELL_SCHOOL_MASK_NONE = &H0
        SPELL_SCHOOL_MASK_NORMAL = &H1
        SPELL_SCHOOL_MASK_HOLY = &H2
        SPELL_SCHOOL_MASK_FIRE = &H4
        SPELL_SCHOOL_MASK_NATURE = &H8
        SPELL_SCHOOL_MASK_FROST = &H10
        SPELL_SCHOOL_MASK_SHADOW = &H20
        SPELL_SCHOOL_MASK_ARCANE = &H40
        SPELL_SCHOOL_MASK_SPELL = (SPELL_SCHOOL_MASK_FIRE Or SPELL_SCHOOL_MASK_NATURE Or SPELL_SCHOOL_MASK_FROST Or SPELL_SCHOOL_MASK_SHADOW Or SPELL_SCHOOL_MASK_ARCANE)
        SPELL_SCHOOL_MASK_MAGIC = (SPELL_SCHOOL_MASK_HOLY Or SPELL_SCHOOL_MASK_SPELL)
        SPELL_SCHOOL_MASK_ALL = (SPELL_SCHOOL_MASK_NORMAL Or SPELL_SCHOOL_MASK_MAGIC)
    End Enum
    Public Enum SpellAuraInterruptFlags As Integer
        AURA_INTERRUPT_FLAG_UNK0 = &H1
        AURA_INTERRUPT_FLAG_DAMAGE = &H2 'removed by any damage
        AURA_INTERRUPT_FLAG_UNK2 = &H4
        AURA_INTERRUPT_FLAG_MOVE = &H8 'removed by any movement
        AURA_INTERRUPT_FLAG_TURNING = &H10 'removed by any turning
        AURA_INTERRUPT_FLAG_ENTER_COMBAT = &H20 'removed by entering combat
        AURA_INTERRUPT_FLAG_NOT_MOUNTED = &H40 'removed by unmounting
        AURA_INTERRUPT_FLAG_NOT_ABOVEWATER = &H80 'removed by entering water
        AURA_INTERRUPT_FLAG_NOT_UNDERWATER = &H100 'removed by leaving water
        AURA_INTERRUPT_FLAG_NOT_SHEATHED = &H200 'removed by unsheathing
        AURA_INTERRUPT_FLAG_UNK10 = &H400
        AURA_INTERRUPT_FLAG_UNK11 = &H800
        AURA_INTERRUPT_FLAG_UNK12 = &H1000 'removed by attack?
        AURA_INTERRUPT_FLAG_UNK13 = &H2000
        AURA_INTERRUPT_FLAG_UNK14 = &H4000
        AURA_INTERRUPT_FLAG_UNK15 = &H8000
        AURA_INTERRUPT_FLAG_UNK16 = &H10000
        AURA_INTERRUPT_FLAG_MOUNTING = &H20000 'removed by mounting
        AURA_INTERRUPT_FLAG_NOT_SEATED = &H40000 'removed by standing up
        AURA_INTERRUPT_FLAG_CHANGE_MAP = &H80000 'leaving map/getting teleported
        AURA_INTERRUPT_FLAG_UNK20 = &H100000
        AURA_INTERRUPT_FLAG_UNK21 = &H200000
        AURA_INTERRUPT_FLAG_UNK22 = &H400000
        AURA_INTERRUPT_FLAG_ENTER_PVP_COMBAT = &H800000 'removed by entering pvp combat
        AURA_INTERRUPT_FLAG_DIRECT_DAMAGE = &H1000000 'removed by any direct damage
    End Enum
    Public Enum SpellAuraProcFlags As Integer
        AURA_PROC_NULL = &H0
        AURA_PROC_ON_ANY_HOSTILE_ACTION = &H1
        AURA_PROC_ON_GAIN_EXPIERIENCE = &H2
        AURA_PROC_ON_MELEE_ATTACK = &H4
        AURA_PROC_ON_CRIT_HIT_VICTIM = &H8
        AURA_PROC_ON_CAST_SPELL = &H10
        AURA_PROC_ON_PHYSICAL_ATTACK_VICTIM = &H20
        AURA_PROC_ON_RANGED_ATTACK = &H40
        AURA_PROC_ON_RANGED_CRIT_ATTACK = &H80
        AURA_PROC_ON_PHYSICAL_ATTACK = &H100
        AURA_PROC_ON_MELEE_ATTACK_VICTIM = &H200
        AURA_PROC_ON_SPELL_HIT = &H400
        AURA_PROC_ON_RANGED_CRIT_ATTACK_VICTIM = &H800
        AURA_PROC_ON_CRIT_ATTACK = &H1000
        AURA_PROC_ON_RANGED_ATTACK_VICTIM = &H2000
        AURA_PROC_ON_PRE_DISPELL_AURA_VICTIM = &H4000
        AURA_PROC_ON_SPELL_LAND_VICTIM = &H8000
        AURA_PROC_ON_CAST_SPECIFIC_SPELL = &H10000
        AURA_PROC_ON_SPELL_HIT_VICTIM = &H20000
        AURA_PROC_ON_SPELL_CRIT_HIT_VICTIM = &H40000
        AURA_PROC_ON_TARGET_DIE = &H80000
        AURA_PROC_ON_ANY_DAMAGE_VICTIM = &H100000
        AURA_PROC_ON_TRAP_TRIGGER = &H200000                'triggers on trap activation
        AURA_PROC_ON_AUTO_SHOT_HIT = &H400000
        AURA_PROC_ON_ABSORB = &H800000
        AURA_PROC_ON_RESIST_VICTIM = &H1000000
        AURA_PROC_ON_DODGE_VICTIM = &H2000000
        AURA_PROC_ON_DIE = &H4000000
        AURA_PROC_REMOVEONUSE = &H8000000                   'remove AURA_PROChcharge only when it is used
        AURA_PROC_MISC = &H10000000                          'our custom flag to decide if AURA_PROC dmg or shield
        AURA_PROC_ON_BLOCK_VICTIM = &H20000000
        AURA_PROC_ON_SPELL_CRIT_HIT = &H40000000
        AURA_PROC_TARGET_SELF = &H80000000                   'our custom flag to decide if AURA_PROC target is self or victim
    End Enum
    Public Enum AuraTickFlags As Byte
        FLAG_PERIODIC_DAMAGE = &H2
        FLAG_PERIODIC_TRIGGER_SPELL = &H4
        FLAG_PERIODIC_HEAL = &H8
        FLAG_PERIODIC_LEECH = &H10
        FLAG_PERIODIC_ENERGIZE = &H20
    End Enum
    Public Enum SpellProcFlags As Byte
        PROC_ON_DAMAGE_RECEIVED = 3
    End Enum
    Public Enum SpellCastFlags As Short
        CAST_FLAG_UNKNOWN1 = &H2
        CAST_FLAG_UNKNOWN2 = &H10
        CAST_FLAG_AMMO = &H20
        CAST_FLAG_UNKNOWN3 = &H100
    End Enum
    Public Enum SpellAttributes As Integer
        SPELL_UNK0 = &H1                                    ' 0
        SPELL_RANGED = &H2                                  ' 1 All ranged abilites have this flag
        SPELL_ON_NEXT_SWING_1 = &H4                         ' 2 on next swing
        SPELL_UNK3 = &H8                                    ' 3 not set in 2.4.2
        SPELL_UNK4 = &H10                                   ' 4
        SPELL_UNK5 = &H20                                   ' 5 trade spells?
        SPELL_PASSIVE = &H40                                ' 6 Passive spell
        SPELL_UNK7 = &H80                                   ' 7 visible?
        SPELL_UNK8 = &H100                                  ' 8
        SPELL_UNK9 = &H200                                  ' 9
        SPELL_ON_NEXT_SWING_2 = &H400                       ' 10 on next swing 2
        SPELL_UNK11 = &H800                                 ' 11
        SPELL_DAYTIME_ONLY = &H1000                         ' 12 only useable at daytime, not set in 2.4.2
        SPELL_NIGHT_ONLY = &H2000                           ' 13 only useable at night, not set in 2.4.2
        SPELL_INDOORS_ONLY = &H4000                         ' 14 only useable indoors, not set in 2.4.2
        SPELL_OUTDOORS_ONLY = &H8000                        ' 15 Only useable outdoors.
        SPELL_NOT_SHAPESHIFT = &H10000                      ' 16 Not while shapeshifted
        SPELL_ONLY_STEALTHED = &H20000                      ' 17 Must be in stealth
        SPELL_UNK18 = &H40000                               ' 18
        SPELL_UNK19 = &H80000                               ' 19
        SPELL_STOP_ATTACK_TARGET = &H100000                 ' 20 Stop attack after use this spell (and not begin attack if use)
        SPELL_IMPOSSIBLE_DODGE_PARRY_BLOCK = &H200000       ' 21 Cannot be dodged/parried/blocked
        SPELL_UNK22 = &H400000                              ' 22
        SPELL_UNK23 = &H800000                              ' 23 castable while dead?
        SPELL_CASTABLE_WHILE_MOUNTED = &H1000000            ' 24 castable while mounted
        SPELL_DISABLED_WHILE_ACTIVE = &H2000000             ' 25 Activate and start cooldown after aura fade or remove summoned creature or go
        SPELL_UNK26 = &H4000000                             ' 26
        SPELL_CASTABLE_WHILE_SITTING = &H8000000            ' 27 castable while sitting
        SPELL_CANT_USED_IN_COMBAT = &H10000000              ' 28 Cannot be used in combat
        SPELL_UNAFFECTED_BY_INVULNERABILITY = &H20000000    ' 29 unaffected by invulnerability (hmm possible not...)
        SPELL_UNK30 = &H40000000                            ' 30 breakable by damage?
        SPELL_UNK31 = &H80000000                            ' 31
    End Enum
    Public Enum SpellAttributesEx As Integer
        SPELL_ATTR_EX_DRAIN_ALL_POWER = &H2 'use all power (Only paladin Lay of Hands and Bunyanize)
        SPELL_ATTR_EX_CHANNELED_1 = &H4 'channeled 1
        SPELL_ATTR_EX_NOT_BREAK_STEALTH = &H20 'Not break stealth
        SPELL_ATTR_EX_CHANNELED_2 = &H40 'channeled 2
        SPELL_ATTR_EX_NEGATIVE = &H80 'negative spell?
        SPELL_ATTR_EX_NOT_IN_COMBAT_TARGET = &H100 'Spell req target not to be in combat state
        SPELL_ATTR_EX_DISPEL_AURAS_ON_IMMUNITY = &H8000 'remove auras on immunity
        SPELL_ATTR_EX_UNAFFECTED_BY_SCHOOL_IMMUNE = &H10000 'unaffected by school immunity
        SPELL_ATTR_EX_REQ_COMBO_POINTS1 = &H100000 'Req combo points on target
        SPELL_ATTR_EX_REQ_COMBO_POINTS2 = &H400000 'Req combo points on target
    End Enum
    Public Enum SpellAttributesEx2 As Integer
        SPELL_ATTR_EX2_AUTO_SHOOT = &H20 'Auto Shoot?
        SPELL_ATTR_EX2_HEALTH_FUNNEL = &H800 'Health funnel pets?
        SPELL_ATTR_EX2_NOT_NEED_SHAPESHIFT = &H80000 'does not necessarly need shapeshift
        SPELL_ATTR_EX2_CANT_CRIT = &H20000000 'Spell can't crit
    End Enum
    Public Enum SpellAttributesEx3 As Integer
        SPELL_ATTR_EX3_MAIN_HAND = &H400 'Main hand weapon required
        SPELL_ATTR_EX3_BATTLEGROUND = &H80 'Can casted only on battleground
        SPELL_ATTR_EX3_NO_HONOR = &H4000 'No honor on kill
        SPELL_ATTR_EX3_AUTO_SHOOT = &H8000 'Auto shoot
        SPELL_ATTR_EX3_NO_INITIAL_AGGRO = &H20000 'no initial aggro
        SPELL_ATTR_EX3_DEATH_PERSISTENT = &H100000 'Death persistent spells
        SPELL_ATTR_EX3_REQ_WAND = &H400000 'Req wand
        SPELL_ATTR_EX3_REQ_OFFHAND = &H1000000 'Req offhand weapon
    End Enum
    Public Enum SpellAttributesEx4 As Integer
        SPELL_ATTR_EX4_SPELL_VS_EXTEND_COST = &H400 'Rogue Shiv have this flag
        SPELL_ATTR_EX4_CAST_ONLY_IN_OUTLAND = &H4000000 'Can only be used in Outland.
    End Enum
    Public Enum SpellAttributesEx5 As Integer
        SPELL_ATTR_EX5_USABLE_WHILE_STUNNED = &H8 'usable while stunned
        SPELL_ATTR_EX5_SINGLE_TARGET_SPELL = &H20 'Only one target can be apply at a time
        SPELL_ATTR_EX5_USABLE_WHILE_FEARED = &H20000 'usable while feared
        SPELL_ATTR_EX5_USABLE_WHILE_CONFUSED = &H40000 'usable while confused
    End Enum

    Public Enum SpellCastTargetFlags As Integer
        TARGET_FLAG_SELF = &H0
        TARGET_FLAG_UNIT = &H2              'Swimmers
        TARGET_FLAG_ITEM = &H1010           'Item -&H10?
        TARGET_FLAG_SOURCE_LOCATION = &H20  'PBAE
        TARGET_FLAG_DEST_LOCATION = &H40    'Targeted AE
        TARGET_FLAG_OBJECT = &H800
        TARGET_FLAG_STRING = &H2000

        'TARGET_FLAG_PLAYER_CORPSE = &H200  'Player Corpse
        'TARGET_FLAG_SELF = &H100           'Self
        'TARGET_FLAG_UNK =&H400             'Mass Spirit Heal
        'TARGET_FLAG_OBJECT=&H4000          'World Object
        'TARGET_FLAG_RESS_CORPSE=&H8000     'Resurrectable Corpse
    End Enum
    Public Enum SpellFailedReason As Byte
        CAST_NO_ERROR = 255
        CAST_FAIL_AFFECTING_COMBAT = 0
        CAST_FAIL_ALREADY_AT_FULL_HEALTH = 1
        CAST_FAIL_ALREADY_AT_FULL_MANA = 2
        CAST_FAIL_ALREADY_AT_FULL_POWER = 3
        CAST_FAIL_ALREADY_BEING_TAMED = 4
        CAST_FAIL_ALREADY_HAVE_CHARM = 5
        CAST_FAIL_ALREADY_HAVE_SUMMON = 6
        CAST_FAIL_ALREADY_OPEN = 7
        CAST_FAIL_AURA_BOUNCED = 8
        CAST_FAIL_AUTOTRACK_INTERRUPTED = 9
        CAST_FAIL_BAD_IMPLICIT_TARGETS = 10
        CAST_FAIL_BAD_TARGETS = 11
        CAST_FAIL_CANT_BE_CHARMED = 12
        CAST_FAIL_CANT_BE_DISENCHANTED = 13
        CAST_FAIL_CANT_BE_DISENCHANTED_SKILL = 14
        CAST_FAIL_CANT_BE_PROSPECTED = 15
        CAST_FAIL_CANT_CAST_ON_TAPPED = 16
        CAST_FAIL_CANT_DUEL_WHILE_INVISIBLE = 17
        CAST_FAIL_CANT_DUEL_WHILE_STEALTHED = 18
        CAST_FAIL_CANT_STEALTH = 19
        CAST_FAIL_CASTER_AURASTATE = 20
        CAST_FAIL_CASTER_DEAD = 21
        CAST_FAIL_CHARMED = 22
        CAST_FAIL_CHEST_IN_USE = 23
        CAST_FAIL_CONFUSED = 24
        CAST_FAIL_DONT_REPORT = 25
        CAST_FAIL_EQUIPPED_ITEM = 26
        CAST_FAIL_EQUIPPED_ITEM_CLASS = 27
        CAST_FAIL_EQUIPPED_ITEM_CLASS_MAINHAND = 28
        CAST_FAIL_EQUIPPED_ITEM_CLASS_OFFHAND = 29
        CAST_FAIL_ERROR = 30
        CAST_FAIL_FIZZLE = 31
        CAST_FAIL_FLEEING = 32
        CAST_FAIL_FOOD_LOWLEVEL = 33
        CAST_FAIL_HIGHLEVEL = 34
        CAST_FAIL_HUNGER_SATIATED = 35
        CAST_FAIL_IMMUNE = 36
        CAST_FAIL_INTERRUPTED = 37
        CAST_FAIL_INTERRUPTED_COMBAT = 38
        CAST_FAIL_ITEM_ALREADY_ENCHANTED = 39
        CAST_FAIL_ITEM_GONE = 40
        CAST_FAIL_ITEM_NOT_FOUND = 41
        CAST_FAIL_ITEM_NOT_READY = 42
        CAST_FAIL_LEVEL_REQUIREMENT = 43
        CAST_FAIL_LINE_OF_SIGHT = 44
        CAST_FAIL_LOWLEVEL = 45
        CAST_FAIL_LOW_CASTLEVEL = 46
        CAST_FAIL_MAINHAND_EMPTY = 47
        CAST_FAIL_MOVING = 48
        CAST_FAIL_NEED_AMMO = 49
        CAST_FAIL_NEED_AMMO_POUCH = 50
        CAST_FAIL_NEED_EXOTIC_AMMO = 51
        CAST_FAIL_NOPATH = 52
        CAST_FAIL_NOT_BEHIND = 53
        CAST_FAIL_NOT_FISHABLE = 54
        CAST_FAIL_NOT_FLYING = 55
        CAST_FAIL_NOT_HERE = 56
        CAST_FAIL_NOT_INFRONT = 57
        CAST_FAIL_NOT_IN_CONTROL = 58
        CAST_FAIL_NOT_KNOWN = 59
        CAST_FAIL_NOT_MOUNTED = 60
        CAST_FAIL_NOT_ON_TAXI = 61
        CAST_FAIL_NOT_ON_TRANSPORT = 62
        CAST_FAIL_NOT_READY = 63
        CAST_FAIL_NOT_SHAPESHIFT = 64
        CAST_FAIL_NOT_STANDING = 65
        CAST_FAIL_NOT_TRADEABLE = 66
        CAST_FAIL_NOT_TRADING = 67
        CAST_FAIL_NOT_UNSHEATHED = 68
        CAST_FAIL_NOT_WHILE_GHOST = 69
        CAST_FAIL_NO_AMMO = 70
        CAST_FAIL_NO_CHARGES_REMAIN = 71
        CAST_FAIL_NO_CHAMPION = 72
        CAST_FAIL_NO_COMBO_POINTS = 73
        CAST_FAIL_NO_DUELING = 74
        CAST_FAIL_NO_ENDURANCE = 75
        CAST_FAIL_NO_FISH = 76
        CAST_FAIL_NO_ITEMS_WHILE_SHAPESHIFTED = 77
        CAST_FAIL_NO_MOUNTS_ALLOWED = 78
        CAST_FAIL_NO_PET = 79
        CAST_FAIL_NO_POWER = 80
        CAST_FAIL_NOTHING_TO_DISPEL = 81
        CAST_FAIL_NOTHING_TO_STEAL = 82
        CAST_FAIL_ONLY_ABOVEWATER = 83
        CAST_FAIL_ONLY_DAYTIME = 84
        CAST_FAIL_ONLY_INDOORS = 85
        CAST_FAIL_ONLY_MOUNTED = 86
        CAST_FAIL_ONLY_NIGHTTIME = 87
        CAST_FAIL_ONLY_OUTDOORS = 88
        CAST_FAIL_ONLY_SHAPESHIFT = 89
        CAST_FAIL_ONLY_STEALTHED = 90
        CAST_FAIL_ONLY_UNDERWATER = 91
        CAST_FAIL_OUT_OF_RANGE = 92
        CAST_FAIL_PACIFIED = 93
        CAST_FAIL_POSSESSED = 94
        CAST_FAIL_REAGENTS = 95
        CAST_FAIL_REQUIRES_AREA = 96
        CAST_FAIL_REQUIRES_SPELL_FOCUS = 97
        CAST_FAIL_ROOTED = 98
        CAST_FAIL_SILENCED = 99
        CAST_FAIL_SPELL_IN_PROGRESS = 100
        CAST_FAIL_SPELL_LEARNED = 101
        CAST_FAIL_SPELL_UNAVAILABLE = 102
        CAST_FAIL_STUNNED = 103
        CAST_FAIL_TARGETS_DEAD = 104
        CAST_FAIL_TARGET_AFFECTING_COMBAT = 105
        CAST_FAIL_TARGET_AURASTATE = 106
        CAST_FAIL_TARGET_DUELING = 107
        CAST_FAIL_TARGET_ENEMY = 108
        CAST_FAIL_TARGET_ENRAGED = 109
        CAST_FAIL_TARGET_FRIENDLY = 110
        CAST_FAIL_TARGET_IN_COMBAT = 111
        CAST_FAIL_TARGET_IS_PLAYER = 112
        CAST_FAIL_TARGET_IS_PLAYER_CONTROLLED = 113
        CAST_FAIL_TARGET_NOT_DEAD = 114
        CAST_FAIL_TARGET_NOT_IN_PARTY = 115
        CAST_FAIL_TARGET_NOT_LOOTED = 116
        CAST_FAIL_TARGET_NOT_PLAYER = 117
        CAST_FAIL_TARGET_NO_POCKETS = 118
        CAST_FAIL_TARGET_NO_WEAPONS = 119
        CAST_FAIL_TARGET_UNSKINNABLE = 120
        CAST_FAIL_THIRST_SATIATED = 121
        CAST_FAIL_TOO_CLOSE = 122
        CAST_FAIL_TOO_MANY_OF_ITEM = 123
        CAST_FAIL_TOTEM_CATEGORY = 124
        CAST_FAIL_TOTEMS = 125
        CAST_FAIL_TRAINING_POINTS = 126
        CAST_FAIL_TRY_AGAIN = 127
        CAST_FAIL_UNIT_NOT_BEHIND = 128
        CAST_FAIL_UNIT_NOT_INFRONT = 129
        CAST_FAIL_WRONG_PET_FOOD = 130
        CAST_FAIL_NOT_WHILE_FATIGUED = 131
        CAST_FAIL_TARGET_NOT_IN_INSTANCE = 132
        CAST_FAIL_NOT_WHILE_TRADING = 133
        CAST_FAIL_TARGET_NOT_IN_RAID = 134
        CAST_FAIL_DISENCHANT_WHILE_LOOTING = 135
        CAST_FAIL_PROSPECT_WHILE_LOOTING = 136
        CAST_FAIL_PROSPECT_NEED_MORE = 137
        CAST_FAIL_TARGET_FREEFORALL = 138
        CAST_FAIL_NO_EDIBLE_CORPSES = 139
        CAST_FAIL_ONLY_BATTLEGROUNDS = 140
        CAST_FAIL_TARGET_NOT_GHOST = 141
        CAST_FAIL_TOO_MANY_SKILLS = 142
        CAST_FAIL_TRANSFORM_UNUSABLE = 143
        CAST_FAIL_WRONG_WEATHER = 144
        CAST_FAIL_DAMAGE_IMMUNE = 145
        CAST_FAIL_PREVENTED_BY_MECHANIC = 146
        CAST_FAIL_PLAY_TIME = 147
        CAST_FAIL_REPUTATION = 148
        CAST_FAIL_MIN_SKILL = 149
        CAST_FAIL_NOT_IN_ARENA = 150
        CAST_FAIL_NOT_ON_SHAPESHIFT = 151
        CAST_FAIL_NOT_ON_STEALTHED = 152
        CAST_FAIL_NOT_ON_DAMAGE_IMMUNE = 153
        CAST_FAIL_NOT_ON_MOUNTED = 154
        CAST_FAIL_TOO_SHALLOW = 155
        CAST_FAIL_TARGET_NOT_IN_SANCTUARY = 156
        CAST_FAIL_TARGET_IS_TRIVIAL = 157
        CAST_FAIL_BM_OR_INVISGOD = 158
        CAST_FAIL_EXPERT_RIDING_REQUIREMENT = 159
        CAST_FAIL_ARTISAN_RIDING_REQUIREMENT = 160
        CAST_FAIL_NOT_IDLE = 161
        CAST_FAIL_NOT_INACTIVE = 162
        CAST_FAIL_PARTIAL_PLAYTIME = 163
        CAST_FAIL_NO_PLAYTIME = 164
        CAST_FAIL_NOT_IN_BATTLEGROUND = 165
        CAST_FAIL_ONLY_IN_ARENA = 166
        CAST_FAIL_TARGET_LOCKED_TO_RAID_INSTANCE = 167
        CAST_FAIL_UNKNOWN = 168
    End Enum
    Public Enum SpellImplicitTargets As Byte
        TARGET_NOTHING = 0

        TARGET_SELF = 1
        TARGET_PET = 5
        TARGET_SELECTED_ENEMY = 6
        TARGET_AREA_EFFECT_ENEMY = 15
        TARGET_AREA_EFFECT_ENEMY_INSTANT = 16

        TARGET_AROUND_CASTER_PARTY = 20
        TARGET_SELECTED_FRIEND = 21
        TARGET_AROUND_CASTER_ENEMY = 22
        TARGET_SELECTED_GAMEOBJECT = 23
        TARGET_INFRONT = 24
        TARGET_DUEL_VS_PLAYER = 25                  'Used when part of spell is casted on another target
        TARGET_GAMEOBJECT_AND_ITEM = 26
        TARGET_MASTER = 27      'not tested
        TARGET_AREA_EFFECT_ENEMY_CHANNEL = 28

        TARGET_MINION = 32                          'Summons your pet to you.
        TARGET_SELECTED_PLAYER = 35                 'Surrounds the target in a shield of fire.
        TARGET_SELECTED_PLAYER_PARTY = 37           'Power infuses the target's party, increasing their Shadow resistance by $s1 for $d.
        TARGET_CASTER = 38                          'Enegy used returns to the caster.
        TARGET_SELF_FISHING = 39                    'Equip a fishing pole and find a body of water to fish.
        TARGET_TOTEM_EARTH = 41                     'Summons a Stoneclaw Totem.
        TARGET_TOTEM_WATER = 42                     'Summons a Healing Stream Totem.
        TARGET_TOTEM_AIR = 43                       'Summons a Windfury Totem.
        TARGET_TOTEM_FIRE = 44                      'Summons a Frost Resistance Totem.
        TARGET_CHAIN = 45                           'Heals the friendly target for $s1, then jumps to heal additional nearby targets.
        TARGET_DYNAMIC_OBJECT = 47
        TARGET_AREA_EFFECT_SELECTED = 53            'Inflicts $s1 Fire damage to all enemies in a selected area.
        TARGET_UNK54 = 54
        TARGET_SELECTED_PARTY_MEMBER = 57           'Puts a pirate costume on targeted party member.

    End Enum

    Public Enum ShapeshiftForm As Byte
        FORM_NORMAL = 0

        FORM_CAT = 1
        FORM_TREE = 2
        FORM_TRAVEL = 3
        FORM_AQUA = 4
        FORM_BEAR = 5
        FORM_AMBIENT = 6
        FORM_GHOUL = 7
        FORM_DIREBEAR = 8
        FORM_CREATUREBEAR = 14
        FORM_CREATURECAT = 15
        FORM_GHOSTWOLF = 16
        FORM_BATTLESTANCE = 17
        FORM_DEFENSIVESTANCE = 18
        FORM_BERSERKERSTANCE = 19
        FORM_SWIFT = 27
        FORM_SHADOW = 28
        FORM_FLIGHT = 29
        FORM_STEALTH = 30
        FORM_MOONKIN = 31
        FORM_SPIRITOFREDEMPTION = 32
    End Enum

    Public Enum SummonType
        SUMMON_TYPE_CRITTER = 41
        SUMMON_TYPE_GUARDIAN = 61
        SUMMON_TYPE_TOTEM_SLOT1 = 63
        SUMMON_TYPE_WILD = 64
        SUMMON_TYPE_POSESSED = 65
        SUMMON_TYPE_DEMON = 66
        SUMMON_TYPE_SUMMON = 67
        SUMMON_TYPE_TOTEM_SLOT2 = 81
        SUMMON_TYPE_TOTEM_SLOT3 = 82
        SUMMON_TYPE_TOTEM_SLOT4 = 83
        SUMMON_TYPE_TOTEM = 121
        SUMMON_TYPE_UNKNOWN3 = 181
        SUMMON_TYPE_UNKNOWN4 = 187
        SUMMON_TYPE_UNKNOWN1 = 247
        SUMMON_TYPE_UNKNOWN5 = 307
        SUMMON_TYPE_CRITTER2 = 407
        SUMMON_TYPE_UNKNOWN6 = 409
        SUMMON_TYPE_UNKNOWN2 = 427
        SUMMON_TYPE_POSESSED2 = 428
    End Enum

    Public Enum TempSummonType
        TEMPSUMMON_TIMED_OR_DEAD_DESPAWN = 1             'despawns after a specified time OR when the creature disappears
        TEMPSUMMON_TIMED_OR_CORPSE_DESPAWN = 2             'despawns after a specified time OR when the creature dies
        TEMPSUMMON_TIMED_DESPAWN = 3             'despawns after a specified time
        TEMPSUMMON_TIMED_DESPAWN_OUT_OF_COMBAT = 4             'despawns after a specified time after the creature is out of combat
        TEMPSUMMON_CORPSE_DESPAWN = 5             'despawns instantly after death
        TEMPSUMMON_CORPSE_TIMED_DESPAWN = 6             'despawns after a specified time after death
        TEMPSUMMON_DEAD_DESPAWN = 7             'despawns when the creature disappears
        TEMPSUMMON_MANUAL_DESPAWN = 8              'despawns when UnSummon() is called
    End Enum

    Public Function GetShapeshiftModel(ByVal form As ShapeshiftForm, ByVal race As Races, ByVal model As Integer) As Integer
        Select Case form
            Case ShapeshiftForm.FORM_CAT
                If race = Races.RACE_NIGHT_ELF Then Return 892
                If race = Races.RACE_TAUREN Then Return 8571
            Case ShapeshiftForm.FORM_BEAR, ShapeshiftForm.FORM_DIREBEAR
                If race = Races.RACE_NIGHT_ELF Then Return 2281
                If race = Races.RACE_TAUREN Then Return 2289
            Case ShapeshiftForm.FORM_MOONKIN
                If race = Races.RACE_NIGHT_ELF Then Return 15374
                If race = Races.RACE_TAUREN Then Return 15375
            Case ShapeshiftForm.FORM_TRAVEL
                Return 632
            Case ShapeshiftForm.FORM_AQUA
                Return 2428
            Case ShapeshiftForm.FORM_FLIGHT
                If race = Races.RACE_NIGHT_ELF Then Return 20857
                If race = Races.RACE_TAUREN Then Return 20872
            Case ShapeshiftForm.FORM_SWIFT
                If race = Races.RACE_NIGHT_ELF Then Return 21243
                If race = Races.RACE_TAUREN Then Return 21244
            Case ShapeshiftForm.FORM_GHOUL
                If race = Races.RACE_NIGHT_ELF Then Return 10045 Else Return model
            Case ShapeshiftForm.FORM_CREATUREBEAR
                Return 902
            Case ShapeshiftForm.FORM_GHOSTWOLF
                Return 4613
            Case ShapeshiftForm.FORM_TREE
                Return 864
            Case ShapeshiftForm.FORM_SPIRITOFREDEMPTION
                Return 12824
            Case Else
                Return model
                'Case ShapeshiftForm.FORM_CREATURECAT
                'Case ShapeshiftForm.FORM_AMBIENT
                'Case ShapeshiftForm.FORM_SHADOW
        End Select
    End Function
    Public Function GetShapeshiftManaType(ByVal form As ShapeshiftForm, ByVal manaType As ManaTypes) As ManaTypes
        Select Case form
            Case ShapeshiftForm.FORM_CAT, ShapeshiftForm.FORM_STEALTH
                Return WS_CharManagment.ManaTypes.TYPE_ENERGY
            Case ShapeshiftForm.FORM_AQUA, ShapeshiftForm.FORM_TRAVEL, ShapeshiftForm.FORM_MOONKIN, ShapeshiftForm.FORM_TREE, _
                 ShapeshiftForm.FORM_MOONKIN, ShapeshiftForm.FORM_MOONKIN, ShapeshiftForm.FORM_SPIRITOFREDEMPTION, ShapeshiftForm.FORM_FLIGHT, ShapeshiftForm.FORM_SWIFT
                Return WS_CharManagment.ManaTypes.TYPE_MANA
            Case ShapeshiftForm.FORM_BEAR, ShapeshiftForm.FORM_DIREBEAR
                Return WS_CharManagment.ManaTypes.TYPE_RAGE
            Case Else
                Return manaType
        End Select
    End Function

#End Region
#Region "WS.Spells.Framework"

    'WARNING: Use only with SPELLs()
    Public Class SpellInfo
        Public ID As Integer = 0
        Public School As Integer = 0
        Public Category As Integer = 0
        Public DispellType As Integer = 0
        Public Mechanic As Integer = 0

        Public Attributes As Integer = 0
        Public AttributesEx As Integer = 0
        Public AttributesEx2 As Integer = 0
        Public AttributesEx3 As Integer = 0
        Public AttributesEx4 As Integer = 0
        Public RequredCasterStance As Integer = 0
        Public ShapeshiftExclude As Integer = 0
        Public Target As Integer = 0
        Public TargetCreatureType As Integer = 0
        Public FocusObjectIndex As Integer = 0
        Public FacingCasterFlags As Integer = 0
        Public CasterAuraState As Integer = 0
        Public TargetAuraState As Integer = 0
        Public ExcludeCasterAuraState As Integer = 0
        Public ExcludeTargetAuraState As Integer = 0

        Public SpellCastTimeIndex As Integer = 0
        Public CategoryCooldown As Integer = 0
        Public SpellCooldown As Integer = 0

        Public interruptFlags As Integer = 0
        Public auraInterruptFlags As Integer = 0
        Public channelInterruptFlags As Integer = 0
        Public procFlags As Integer = 0
        Public procChance As Integer = 0
        Public procCharges As Integer = 0
        Public maxLevel As Integer = 0
        Public baseLevel As Integer = 0
        Public spellLevel As Integer = 0
        Public maxStack As Integer = 0

        Public DurationIndex As Integer = 0

        Public powerType As Integer = 0
        Public manaCost As Integer = 0
        Public manaCostPerlevel As Integer = 0
        Public manaPerSecond As Integer = 0
        Public manaPerSecondPerLevel As Integer = 0
        Public manaCostPercent As Integer = 0

        Public rangeIndex As Integer = 0
        Public Speed As Single = 0
        Public modalNextSpell As Integer = 0

        Public Totem() As Integer = {0, 0}
        Public TotemCategory() As Integer = {0, 0}
        Public Reagents() As Integer = {0, 0, 0, 0, 0, 0, 0, 0}
        Public ReagentsCount() As Integer = {0, 0, 0, 0, 0, 0, 0, 0}

        Public EquippedItemClass As Integer = 0
        Public EquippedItemSubClass As Integer = 0
        Public EquippedItemInventoryType As Integer = 0

        Public SpellEffects() As SpellEffect = {Nothing, Nothing, Nothing}

        Public MaxTargets As Integer = 0
        Public RequiredAreaID As Integer = 0

        Public SpellVisual As Integer = 0
        Public DamageType As Integer = 0
        Public Name As String = ""

        Public ReadOnly Property GetDuration() As Integer
            Get
                If SpellDuration.ContainsKey(DurationIndex) Then Return SpellDuration(DurationIndex)
                Return 0
            End Get
        End Property
        Public ReadOnly Property GetRange() As Integer
            Get
                If SpellRange.ContainsKey(rangeIndex) Then Return SpellRange(rangeIndex)
                Return 0
            End Get
        End Property
        Public ReadOnly Property GetFocusObject() As String
            Get
                If SpellFocusObject.ContainsKey(FocusObjectIndex) Then Return SpellFocusObject(FocusObjectIndex)
                Return 0
            End Get
        End Property
        Public ReadOnly Property GetCastTime() As Integer
            Get
                If SpellCastTime.ContainsKey(SpellCastTimeIndex) Then Return SpellCastTime(SpellCastTimeIndex)
                Return 0
            End Get
        End Property
        Public ReadOnly Property GetManaCost(ByVal level As Integer, ByVal Mana As Integer) As Integer
            Get
                Return manaCost + manaCostPerlevel * level + Mana * (manaCostPercent / 100)
            End Get
        End Property
        Public ReadOnly Property IsAura() As Boolean
            Get
                If Not SpellEffects(0) Is Nothing AndAlso SpellEffects(0).ApplyAuraIndex <> 0 Then Return True
                If Not SpellEffects(1) Is Nothing AndAlso SpellEffects(1).ApplyAuraIndex <> 0 Then Return True
                If Not SpellEffects(2) Is Nothing AndAlso SpellEffects(2).ApplyAuraIndex <> 0 Then Return True
                Return False
            End Get
        End Property
        Public ReadOnly Property IsPassive() As Boolean
            Get
                Return (Me.Attributes And SpellAttributes.SPELL_PASSIVE)
            End Get
        End Property
        Public ReadOnly Property IsNegative() As Boolean
            Get
                For i As Byte = 0 To 2
                    If Not SpellEffects(i) Is Nothing AndAlso SpellEffects(i).IsNegative Then Return True
                Next
                Return (Me.AttributesEx And SpellAttributesEx.SPELL_ATTR_EX_NEGATIVE)
            End Get
        End Property
        Public ReadOnly Property IsAutoRepeat() As Boolean
            Get
                Return (Me.AttributesEx2 And SpellAttributesEx2.SPELL_ATTR_EX2_AUTO_SHOOT)
            End Get
        End Property
        Public ReadOnly Property IsRanged() As Boolean
            Get
                Return (Me.Attributes And SpellAttributes.SPELL_RANGED)
            End Get
        End Property

        Public Sub Cast(ByRef caster As BaseObject, ByVal Targets As SpellTargets, ByVal Extra_Cast_Number As Byte, Optional ByVal Item As ItemObject = Nothing, Optional ByVal InstantCast As Boolean = False)
            Dim CastFlags As Integer = SpellCastFlags.CAST_FLAG_UNKNOWN1
            If IsRanged Then CastFlags = CastFlags Or SpellCastFlags.CAST_FLAG_AMMO 'Ranged

            Dim spellStart As New PacketClass(OPCODES.SMSG_SPELL_START)
            'SpellCaster (If the spell is casted by a item, then it's the item guid here, else caster guid)
            If Not Item Is Nothing Then
                spellStart.AddPackGUID(Item.GUID)
            Else
                spellStart.AddPackGUID(caster.GUID)
            End If

            spellStart.AddPackGUID(caster.GUID) 'SpellCaster
            spellStart.AddInt32(ID)
            spellStart.AddInt8(Extra_Cast_Number)
            spellStart.AddInt16(CastFlags)
            If InstantCast Then
                spellStart.AddInt32(0)
            Else
                spellStart.AddInt32(GetCastTime)
            End If
            Targets.WriteTargets(spellStart)

            'DONE: Write ammo to packet
            If (CastFlags And SpellCastFlags.CAST_FLAG_AMMO) Then
                WriteAmmoToPacket(spellStart, caster)
            End If

            If TypeOf caster Is CharacterObject Then CType(caster, CharacterObject).Client.SendMultiplyPackets(spellStart)
            caster.SendToNearPlayers(spellStart)
            spellStart.Dispose()

            'TODO: If ChannelInterruptFlags <>0 then START_CHANNEL

            'PREPEARING SPELL
            Dim tmpRandom As Integer
            If TypeOf caster Is CharacterObject Then
                CType(caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_PREPARING
                CType(caster, CharacterObject).spellCasted = ID
                tmpRandom = Rnd.Next(0, Integer.MaxValue)
                CType(caster, CharacterObject).spellRandom = tmpRandom
            ElseIf TypeOf caster Is CreatureObject Then
                CType(caster, CreatureObject).TurnTo(CType(Targets.unitTarget, BaseUnit))
            End If
            If InstantCast = False Then Thread.Sleep(GetCastTime)

            'CASTING SPELL
            If TypeOf caster Is CharacterObject Then
                If CType(caster, CharacterObject).spellCastState <> SpellCastState.SPELL_STATE_PREPARING Then
                    If CType(caster, CharacterObject).spellCasted > 0 Then 'If the spell isn't already interrupted
                        SendInterrupted(0, caster)
                        SendCastResult(SpellFailedReason.CAST_FAIL_INTERRUPTED, CType(caster, CharacterObject).Client, ID, Extra_Cast_Number)
                        CType(caster, CharacterObject).spellCasted = 0
                        CType(caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_IDLE
                    End If
                    Exit Sub
                ElseIf CType(caster, CharacterObject).spellCasted <> ID OrElse CType(caster, CharacterObject).spellRandom <> tmpRandom Then 'If character is casting a new spell after cancelling another then this will remove the old cancelled spell
                    Exit Sub
                End If
                CType(caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_CASTING
            End If

            'TODO: Apply the damage when the spell has hitted the target (using calculations with spell speed)

            'DONE: Do one more control to see if you still can cast the spell (only if it's not instant)
            Dim SpellCastError As SpellFailedReason = SpellFailedReason.CAST_NO_ERROR
            If (InstantCast = False OrElse GetCastTime = 0) AndAlso TypeOf caster Is CharacterObject Then
                SpellCastError = CanCast(CType(caster, CharacterObject), Targets)
            End If

            'APPLYING EFFECTS
            Dim TargetsInfected As New ArrayList
            If SpellCastError = SpellFailedReason.CAST_NO_ERROR AndAlso Not SpellEffects(0) Is Nothing Then
#If DEBUG Then
                Log.WriteLine(LogType.DEBUG, "DEBUG: Casting effect: {0}", CType(SPELL_EFFECTs(SpellEffects(0).ID), SpellEffectHandler).Method.Name)
#End If
                SpellCastError = SPELL_EFFECTs(SpellEffects(0).ID).Invoke(Targets, caster, SpellEffects(0), ID, TargetsInfected, Item)
            End If
            If SpellCastError = SpellFailedReason.CAST_NO_ERROR AndAlso Not SpellEffects(1) Is Nothing Then
#If DEBUG Then
                Log.WriteLine(LogType.DEBUG, "DEBUG: Casting effect: {0}", CType(SPELL_EFFECTs(SpellEffects(1).ID), SpellEffectHandler).Method.Name)
#End If
                SpellCastError = SPELL_EFFECTs(SpellEffects(1).ID).Invoke(Targets, caster, SpellEffects(1), ID, TargetsInfected, Item)
            End If
            If SpellCastError = SpellFailedReason.CAST_NO_ERROR AndAlso Not SpellEffects(2) Is Nothing Then
#If DEBUG Then
                Log.WriteLine(LogType.DEBUG, "DEBUG: Casting effect: {0}", CType(SPELL_EFFECTs(SpellEffects(2).ID), SpellEffectHandler).Method.Name)
#End If
                SpellCastError = SPELL_EFFECTs(SpellEffects(2).ID).Invoke(Targets, caster, SpellEffects(2), ID, TargetsInfected, Item)
            End If
            If SpellCastError <> SpellFailedReason.CAST_NO_ERROR Then
                If TypeOf caster Is CharacterObject Then
                    SendCastResult(SpellCastError, CType(caster, CharacterObject).Client, ID, Extra_Cast_Number)
                    SendInterrupted(0, caster)
                    CType(caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_IDLE
                    CType(caster, CharacterObject).spellCasted = 0
                    Exit Sub
                Else
                    SendInterrupted(0, caster)
                    Exit Sub
                End If
            End If

            'FINISHED SPELL
            If TypeOf caster Is CharacterObject Then
                'DONE: Get reagents
                For i As Byte = 0 To 7
                    CType(caster, CharacterObject).ItemCONSUME(Reagents(i), ReagentsCount(i))
                Next i

                'DONE: Get mana
                Select Case powerType
                    Case ManaTypes.TYPE_MANA
                        Dim manaCost As Integer = GetManaCost(CType(caster, CharacterObject).Level, CType(caster, CharacterObject).Mana.Base)
                        CType(caster, CharacterObject).Mana.Current -= manaCost
                        'DONE: 5 second rule
                        If manaCost > 0 Then
                            CType(caster, CharacterObject).spellCastManaRegeneration = 5
                            CType(caster, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1, CType(CType(caster, CharacterObject).Mana.Current, Integer))
                        End If
                    Case ManaTypes.TYPE_RAGE
                        CType(caster, CharacterObject).Rage.Current -= GetManaCost(CType(caster, CharacterObject).Level, CType(caster, CharacterObject).Mana.Base)
                        CType(caster, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER2, CType(CType(caster, CharacterObject).Rage.Current, Integer))
                    Case -1
                        CType(caster, CharacterObject).Life.Current -= GetManaCost(CType(caster, CharacterObject).Level, CType(caster, CharacterObject).Mana.Base)
                        CType(caster, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_HEALTH, CType(CType(caster, CharacterObject).Life.Current, Integer))
                    Case ManaTypes.TYPE_ENERGY
                        CType(caster, CharacterObject).Energy.Current -= GetManaCost(CType(caster, CharacterObject).Level, CType(caster, CharacterObject).Mana.Base)
                        CType(caster, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER4, CType(CType(caster, CharacterObject).Energy.Current, Integer))
                End Select
                CType(caster, CharacterObject).SendCharacterUpdate(True)
            End If

            'DONE: Log spell
            SendSpellLog(caster, Targets)

            If TypeOf caster Is CharacterObject Then
                'DONE: Check if the spell was needed for a quest
                If Not (Targets.unitTarget Is Nothing) AndAlso TypeOf Targets.unitTarget Is CreatureObject Then
                    OnQuestCastSpell(CType(caster, CharacterObject), CType(Targets.unitTarget, CreatureObject), ID)
                End If
                If Not (Targets.goTarget Is Nothing) Then
                    OnQuestCastSpell(CType(caster, CharacterObject), CType(Targets.goTarget, GameObjectObject), ID)
                End If

                'DONE: Send cooldown
                SendSpellCooldown(CType(caster, CharacterObject))

                If CType(caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_FINISHED Then
                    CType(caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_IDLE
                    CType(caster, CharacterObject).spellCasted = 0
                    Exit Sub
                End If

                CType(caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_IDLE
                CType(caster, CharacterObject).spellCasted = 0
            End If

            'DONE: Send the GO message
            SendSpellGO(caster, Targets, TargetsInfected, Item)
        End Sub
        Public Sub Apply(ByRef caster As BaseObject, ByVal Targets As SpellTargets)
            Dim TargetsInfected As New ArrayList
            If Not SpellEffects(0) Is Nothing Then SPELL_EFFECTs(SpellEffects(0).ID).Invoke(Targets, caster, SpellEffects(0), ID, TargetsInfected, Nothing)
            If Not SpellEffects(1) Is Nothing Then SPELL_EFFECTs(SpellEffects(1).ID).Invoke(Targets, caster, SpellEffects(1), ID, TargetsInfected, Nothing)
            If Not SpellEffects(2) Is Nothing Then SPELL_EFFECTs(SpellEffects(2).ID).Invoke(Targets, caster, SpellEffects(2), ID, TargetsInfected, Nothing)
        End Sub

        Public Function CanCast(ByRef Character As CharacterObject, ByVal Targets As SpellTargets) As SpellFailedReason
            If (Character.cUnitFlags And UnitFlags.UNIT_FLAG_TAXI_FLIGHT) Then Return SpellFailedReason.CAST_FAIL_ERROR
            If (Not Targets.unitTarget Is Nothing) AndAlso (Not Targets.unitTarget Is Character) Then
                If (FacingCasterFlags And 1) Then
                    If IsInFrontOf(CType(Character, CharacterObject), CType(Targets.unitTarget, BaseUnit)) = False Then Return SpellFailedReason.CAST_FAIL_NOT_INFRONT
                End If
                If (FacingCasterFlags And 2) Then
                    If IsInBackOf(CType(Character, CharacterObject), CType(Targets.unitTarget, BaseUnit)) = False Then Return SpellFailedReason.CAST_FAIL_NOT_BEHIND
                End If
            End If

            If (Attributes And SpellAttributes.SPELL_CANT_USED_IN_COMBAT) Then
                If Character.inCombatWith.Count > 0 Then Return SpellFailedReason.CAST_FAIL_INTERRUPTED_COMBAT
            End If

            Dim StanceMask As Integer = 0
            If Character.ShapeshiftForm > ShapeshiftForm.FORM_NORMAL Then StanceMask = 1 << (CByte(Character.ShapeshiftForm) - 1)
            If (StanceMask And ShapeshiftExclude) Then Return SpellFailedReason.CAST_FAIL_NOT_SHAPESHIFT
            If (StanceMask And RequredCasterStance) = 0 Then
                Dim actAsShifted As Boolean = False
                If Character.ShapeshiftForm > ShapeshiftForm.FORM_NORMAL Then
                    Dim ShapeShift As TSpellShapeshiftForm = FindShapeshiftForm(CInt(Character.ShapeshiftForm))
                    If Not ShapeShift Is Nothing Then
                        If (ShapeShift.Flags1 And 1) = 0 Then
                            actAsShifted = True
                        Else
                            actAsShifted = False
                        End If
                    Else
                        GoTo SkipShapeShift
                    End If
                End If
                If actAsShifted Then
                    If (Attributes And SpellAttributes.SPELL_NOT_SHAPESHIFT) Then
                        Return SpellFailedReason.CAST_FAIL_ONLY_SHAPESHIFT
                    ElseIf RequredCasterStance <> 0 Then
                        Return SpellFailedReason.CAST_FAIL_ONLY_SHAPESHIFT
                    End If
                Else
                    If (AttributesEx2 And SpellAttributesEx2.SPELL_ATTR_EX2_NOT_NEED_SHAPESHIFT) = 0 AndAlso RequredCasterStance <> 0 Then
                        Return SpellFailedReason.CAST_FAIL_ONLY_SHAPESHIFT
                    End If
                End If
            End If

SkipShapeShift:
            If (Attributes And SpellAttributes.SPELL_ONLY_STEALTHED) And Character.Invisibility = InvisibilityLevel.STEALTH Then
                Return SpellFailedReason.CAST_FAIL_ONLY_STEALTHED
            End If

            If (Character.movementFlags And movementFlagsMask) Then
                If ((Character.movementFlags And MovementFlags.MOVEMENTFLAG_FALLING) = 0 OrElse SpellEffects(0).ID <> SpellEffects_Names.SPELL_EFFECT_STUCK) AndAlso (IsAutoRepeat OrElse (auraInterruptFlags And SpellAuraInterruptFlags.AURA_INTERRUPT_FLAG_NOT_SEATED)) Then
                    Return SpellFailedReason.CAST_FAIL_MOVING
                End If
            End If

            If GetManaCost(Character.Level, Character.Mana.Base) <> 0 Then
                If powerType <> CInt(Character.ManaType) Then Return SpellFailedReason.CAST_FAIL_NO_POWER
                Select Case powerType
                    Case ManaTypes.TYPE_MANA
                        If GetManaCost(Character.Level, Character.Mana.Base) > Character.Mana.Current Then Return SpellFailedReason.CAST_FAIL_NO_POWER
                    Case ManaTypes.TYPE_RAGE
                        If GetManaCost(Character.Level, Character.Mana.Base) > Character.Rage.Current Then Return SpellFailedReason.CAST_FAIL_NO_POWER
                    Case -1
                        If GetManaCost(Character.Level, Character.Mana.Base) > Character.Life.Current Then Return SpellFailedReason.CAST_FAIL_NO_POWER
                    Case ManaTypes.TYPE_ENERGY
                        If GetManaCost(Character.Level, Character.Mana.Base) > Character.Energy.Current Then Return SpellFailedReason.CAST_FAIL_NO_POWER
                    Case Else
                        Return SpellFailedReason.CAST_FAIL_UNKNOWN
                End Select
            End If

            If Character.Spell_Silenced Then Return SpellFailedReason.CAST_FAIL_SILENCED

            If Mechanic <> 0 AndAlso Targets.unitTarget.HaveImmuneMechanic(Mechanic) Then Return SpellFailedReason.CAST_FAIL_IMMUNE

            For i As Byte = 0 To 2
                If Not SpellEffects(i) Is Nothing Then
                    Select Case SpellEffects(i).ID
                        Case SpellEffects_Names.SPELL_EFFECT_DUMMY
                            If ID = 1648 Then 'Execute
                                If Targets.unitTarget Is Nothing OrElse Targets.unitTarget.Life.Current > (Targets.unitTarget.Life.Maximum * 0.2) Then Return SpellFailedReason.CAST_FAIL_BAD_TARGETS
                            End If
                        Case SpellEffects_Names.SPELL_EFFECT_SCHOOL_DAMAGE
                            If SpellVisual = 7250 Then 'Hammer of Wrath
                                If Targets.unitTarget Is Nothing Then Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
                                If Targets.unitTarget.Life.Current > (Targets.unitTarget.Life.Maximum * 0.2) Then Return SpellFailedReason.CAST_FAIL_BAD_TARGETS
                            End If
                        Case SpellEffects_Names.SPELL_EFFECT_CHARGE
                            If Character.isRooted Then Return SpellFailedReason.CAST_FAIL_ROOTED
                        Case SpellEffects_Names.SPELL_EFFECT_OPEN_LOCK
                            'TODO: Fix this :P
                    End Select
                End If
            Next

            For i As Byte = 0 To 7
                If Reagents(i) > 0 AndAlso ReagentsCount(i) > 0 Then
                    If Character.ItemCOUNT(Reagents(i)) < ReagentsCount(i) Then Return SpellFailedReason.CAST_FAIL_REAGENTS
                End If
            Next

            'TODO: Check for same category - more powerful spell
            'If (Not Targets.unitTarget Is Nothing) Then
            '    For i As Integer = 0 To MAX_AURA_EFFECTs_VISIBLE - 1
            '        If Not Targets.unitTarget.ActiveSpells(i) Is Nothing Then
            '            If Targets.unitTarget.ActiveSpells(i).SpellID <> 0 AndAlso _
            '                CType(SPELLs(Targets.unitTarget.ActiveSpells(i).SpellID), SpellInfo).Category = Category AndAlso _
            '                CType(SPELLs(Targets.unitTarget.ActiveSpells(i).SpellID), SpellInfo).spellLevel >= spellLevel Then
            '                Return SpellFailedReason.CAST_FAIL_AURA_BOUNCED
            '            End If
            '        End If
            '    Next
            'End If

            Return SpellFailedReason.CAST_NO_ERROR
        End Function
        Public Sub WriteAmmoToPacket(ByRef Packet As PacketClass, ByVal Caster As BaseUnit)
            Dim AmmoDisplayID As Integer = 0
            Dim AmmoInventoryType As Integer = 0
            If TypeOf Caster Is CharacterObject Then
                With CType(Caster, CharacterObject)
                    If .Items.ContainsKey(EQUIPMENT_SLOT_RANGED) Then
                        AmmoInventoryType = .Items(EQUIPMENT_SLOT_RANGED).ItemInfo.InventoryType
                        If AmmoInventoryType = INVENTORY_TYPES.INVTYPE_THROWN Then
                            AmmoDisplayID = .Items(EQUIPMENT_SLOT_RANGED).ItemInfo.Model
                        Else
                            If .AmmoID Then
                                If ITEMDatabase.ContainsKey(.AmmoID) = False Then Dim tmpItem As ItemInfo = New ItemInfo(.AmmoID)
                                AmmoDisplayID = ITEMDatabase(.AmmoID).Model
                                AmmoInventoryType = ITEMDatabase(.AmmoID).InventoryType
                            ElseIf .HavePassiveAura(46699) Then 'Requires no ammo
                                AmmoDisplayID = 5996
                                AmmoInventoryType = INVENTORY_TYPES.INVTYPE_AMMO
                            End If
                        End If
                    End If
                End With
            End If

            CType(Caster, CharacterObject).CommandResponse("AmmoDisplayID: " & AmmoDisplayID)
            CType(Caster, CharacterObject).CommandResponse("AmmoInventoryType: " & AmmoInventoryType)

            Packet.AddInt32(AmmoDisplayID) 'Ammo Display ID
            Packet.AddInt32(AmmoInventoryType) 'Ammo Inventory Type
        End Sub
        Public Sub SendInterrupted(ByVal result As Byte, ByRef Caster As BaseUnit)
            If TypeOf Caster Is CharacterObject Then
                Dim packet As New PacketClass(OPCODES.SMSG_SPELL_FAILURE)
                packet.AddUInt64(Caster.GUID)
                packet.AddInt32(ID)
                packet.AddInt8(result)
                CType(Caster, CharacterObject).Client.Send(packet)
                packet.Dispose()
            End If

            Dim packet2 As New PacketClass(OPCODES.SMSG_SPELL_FAILED_OTHER)
            packet2.AddUInt64(Caster.GUID)
            packet2.AddInt32(ID)
            Caster.SendToNearPlayers(packet2)
            packet2.Dispose()
        End Sub
        Public Sub SendSpellGO(ByRef Caster As BaseObject, ByRef Targets As SpellTargets, ByRef InfectedTargets As ArrayList, ByRef Item As ItemObject)
            Dim castFlags As Short = SpellCastFlags.CAST_FLAG_UNKNOWN3
            If IsRanged Then castFlags = castFlags Or SpellCastFlags.CAST_FLAG_AMMO

            Dim packet As New PacketClass(OPCODES.SMSG_SPELL_GO)
            'SpellCaster (If the spell is casted by a item, then it's the item guid here, else caster guid)
            If Not Item Is Nothing Then
                packet.AddPackGUID(Item.GUID)
            Else
                packet.AddPackGUID(Caster.GUID)
            End If
            packet.AddPackGUID(Caster.GUID)                                 'SpellCaster
            packet.AddInt32(ID)                                             'SpellID
            packet.AddInt16(castFlags)                                      'Flags (&h20 - Ranged, &h100 - Item caster, &h400 - Targets resisted
            packet.AddInt32(timeGetTime)                                    'MsTime
            packet.AddInt8(InfectedTargets.Count)                           'Targets Count
            For Each Target As ULong In InfectedTargets
                packet.AddUInt64(Target)                                     'GUID1...
            Next
            packet.AddInt8(0)                                               'Misses Count
            Targets.WriteTargets(packet)

            'DONE: Write ammo to packet
            If (castFlags And SpellCastFlags.CAST_FLAG_AMMO) Then
                WriteAmmoToPacket(packet, Caster)
            End If

            If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
            Caster.SendToNearPlayers(packet)
            packet.Dispose()
        End Sub
        Public Sub SendSpellLog(ByRef Caster As BaseObject, ByRef Targets As SpellTargets)
            If SpellEffects(0) Is Nothing Then Exit Sub

            Dim packet As New PacketClass(OPCODES.SMSG_SPELLLOGEXECUTE)

            packet.AddPackGUID(Caster.GUID)
            packet.AddInt32(ID)
            packet.AddInt32(1)                                              'uint numOfSpellEffects

            'for(numOfSpellEffects)
            packet.AddInt32(SpellEffects(0).ID)                             'EffID
            packet.AddInt32(1)                                              'EffTargets
            Select Case SpellEffects(0).ID
                Case SpellEffects_Names.SPELL_EFFECT_MANA_DRAIN
                    If Not Targets.unitTarget Is Nothing Then
                        packet.AddPackGUID(Targets.unitTarget.GUID)
                    Else
                        packet.AddInt8(0)
                    End If
                    packet.AddInt32(0)
                    packet.AddInt32(0)
                    packet.AddSingle(0)
                Case SpellEffects_Names.SPELL_EFFECT_ADD_EXTRA_ATTACKS
                    If Not Targets.unitTarget Is Nothing Then
                        packet.AddPackGUID(Targets.unitTarget.GUID)
                    Else
                        packet.AddInt8(0)
                    End If
                    packet.AddInt32(0) 'Count?
                Case SpellEffects_Names.SPELL_EFFECT_INTERRUPT_CAST
                    If Not Targets.unitTarget Is Nothing Then
                        packet.AddPackGUID(Targets.unitTarget.GUID)
                    Else
                        packet.AddInt8(0)
                    End If
                    packet.AddInt32(0) 'SpellID ?
                Case SpellEffects_Names.SPELL_EFFECT_DURABILITY_DAMAGE
                    If Not Targets.unitTarget Is Nothing Then
                        packet.AddPackGUID(Targets.unitTarget.GUID)
                    Else
                        packet.AddInt8(0)
                    End If
                    packet.AddInt32(0)
                    packet.AddInt32(0)
                Case SpellEffects_Names.SPELL_EFFECT_OPEN_LOCK, SpellEffects_Names.SPELL_EFFECT_OPEN_LOCK_ITEM
                    If Not Targets.itemTarget Is Nothing Then
                        packet.AddPackGUID(Targets.itemTarget.GUID)
                    Else
                        packet.AddInt8(0)
                    End If
                Case SpellEffects_Names.SPELL_EFFECT_CREATE_ITEM
                    packet.AddInt32(SpellEffects(0).ItemType)
                Case SpellEffects_Names.SPELL_EFFECT_SUMMON, SpellEffects_Names.SPELL_EFFECT_SUMMON_WILD, SpellEffects_Names.SPELL_EFFECT_SUMMON_GUARDIAN, _
                     SpellEffects_Names.SPELL_EFFECT_SUMMON_OBJECT, SpellEffects_Names.SPELL_EFFECT_SUMMON_PET, SpellEffects_Names.SPELL_EFFECT_SUMMON_POSSESSED, _
                     SpellEffects_Names.SPELL_EFFECT_SUMMON_TOTEM, SpellEffects_Names.SPELL_EFFECT_SUMMON_OBJECT_WILD, SpellEffects_Names.SPELL_EFFECT_CREATE_HOUSE, _
                     SpellEffects_Names.SPELL_EFFECT_DUEL, SpellEffects_Names.SPELL_EFFECT_SUMMON_TOTEM_SLOT1, SpellEffects_Names.SPELL_EFFECT_SUMMON_TOTEM_SLOT2, _
                     SpellEffects_Names.SPELL_EFFECT_SUMMON_TOTEM_SLOT3, SpellEffects_Names.SPELL_EFFECT_SUMMON_TOTEM_SLOT4, SpellEffects_Names.SPELL_EFFECT_SUMMON_PHANTASM, _
                     SpellEffects_Names.SPELL_EFFECT_SUMMON_CRITTER, SpellEffects_Names.SPELL_EFFECT_SUMMON_OBJECT_SLOT1, SpellEffects_Names.SPELL_EFFECT_SUMMON_OBJECT_SLOT2, _
                     SpellEffects_Names.SPELL_EFFECT_SUMMON_OBJECT_SLOT3, SpellEffects_Names.SPELL_EFFECT_SUMMON_OBJECT_SLOT4, SpellEffects_Names.SPELL_EFFECT_SUMMON_DEMON, _
                     SpellEffects_Names.SPELL_EFFECT_150

                    If Not Targets.unitTarget Is Nothing Then
                        packet.AddPackGUID(Targets.unitTarget.GUID)
                    ElseIf Not Targets.itemTarget Is Nothing Then
                        packet.AddPackGUID(Targets.itemTarget.GUID)
                    ElseIf Not Targets.goTarget Is Nothing Then
                        packet.AddPackGUID(Targets.goTarget.GUID)
                    Else
                        packet.AddInt8(0)
                    End If
                Case SpellEffects_Names.SPELL_EFFECT_FEED_PET
                    If Not Targets.itemTarget Is Nothing Then
                        packet.AddInt32(Targets.itemTarget.ItemEntry)
                    Else
                        packet.AddInt32(0)
                    End If
                Case SpellEffects_Names.SPELL_EFFECT_DISMISS_PET
                    If Not Targets.unitTarget Is Nothing Then
                        packet.AddPackGUID(Targets.unitTarget.GUID)
                    Else
                        packet.AddInt8(0)
                    End If
                Case Else
                    packet.Dispose()
                    Exit Sub
            End Select

            If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
            Caster.SendToNearPlayers(packet)
            packet.Dispose()
        End Sub
        Public Sub SendSpellCooldown(ByRef c As CharacterObject)
            'NOTE: Since 2.0.1 this opcode is used instead of SMSG_SPELL_COOLDOWN
            If CategoryCooldown > 0 OrElse SpellCooldown > 0 Then
                Dim packet As New PacketClass(OPCODES.SMSG_COOLDOWN_EVENT)
                packet.AddInt32(ID)                 'SpellID
                packet.AddUInt64(c.GUID)
                c.Client.Send(packet)
                packet.Dispose()
            End If
        End Sub
    End Class
    Public Class SpellEffect
        Public ID As SpellEffects_Names = SpellEffects_Names.SPELL_EFFECT_NOTHING

        Public diceBase As Integer = 0
        Public dicePerLevel As Integer = 0
        Public valueBase As Integer = 0
        Public valueDie As Integer = 0
        Public valuePerLevel As Integer = 0
        Public valuePerComboPoint As Integer = 0
        Public Mechanic As Integer = 0
        Public implicitTargetA As Integer = 0
        Public implicitTargetB As Integer = 0

        Public RadiusIndex As Integer = 0
        Public ApplyAuraIndex As Integer = 0

        Public Amplitude As Integer = 0
        Public ChainTarget As Integer = 0
        Public ItemType As Integer = 0
        Public MiscValue As Integer = 0
        Public MiscValueB As Integer = 0
        Public TriggerSpell As Integer = 0

        Public ReadOnly Property GetRadius() As Integer
            Get
                If SpellRadius.ContainsKey(RadiusIndex) Then Return SpellRadius(RadiusIndex)
                Return 0
            End Get
        End Property
        Public ReadOnly Property GetValue(ByVal Level As Integer, Optional ByVal ComboPoints As Integer = 0) As Integer
            Get
                'Dim baseDamage As Integer = valueBase + (Level * valuePerLevel)
                'Dim randomDamage As Integer = valueDie + (Level * dicePerLevel)
                'Dim comboDamage As Integer = ComboPoints * valuePerComboPoint
                'Return baseDamage + Rnd.Next(1, randomDamage) + comboDamage
                Return valueBase + (Level * valuePerLevel) + ComboPoints * valuePerComboPoint + Rnd.Next(1, valueDie + (Level * dicePerLevel))
            End Get
        End Property
        Public ReadOnly Property IsNegative() As Boolean
            Get
                If ID <> SpellEffects_Names.SPELL_EFFECT_APPLY_AURA Then Return False
                Select Case ApplyAuraIndex
                    Case AuraEffects_Names.SPELL_AURA_GHOST, AuraEffects_Names.SPELL_AURA_MOD_CONFUSE, AuraEffects_Names.SPELL_AURA_MOD_DECREASE_SPEED, AuraEffects_Names.SPELL_AURA_MOD_FEAR
                        Return True
                    Case AuraEffects_Names.SPELL_AURA_MOD_PACIFY, AuraEffects_Names.SPELL_AURA_MOD_PACIFY_SILENCE, AuraEffects_Names.SPELL_AURA_MOD_POSSESS, AuraEffects_Names.SPELL_AURA_PERIODIC_DAMAGE
                        Return True
                    Case AuraEffects_Names.SPELL_AURA_MOD_POSSESS_PET, AuraEffects_Names.SPELL_AURA_MOD_ROOT, AuraEffects_Names.SPELL_AURA_MOD_SILENCE, AuraEffects_Names.SPELL_AURA_MOD_STUN
                        Return True
                    Case AuraEffects_Names.SPELL_AURA_PERIODIC_DAMAGE_PERCENT, AuraEffects_Names.SPELL_AURA_PERIODIC_LEECH, AuraEffects_Names.SPELL_AURA_PERIODIC_MANA_LEECH, AuraEffects_Names.SPELL_AURA_PROC_TRIGGER_DAMAGE
                        Return True
                    Case AuraEffects_Names.SPELL_AURA_TRANSFORM, AuraEffects_Names.SPELL_AURA_SPLIT_DAMAGE_FLAT, AuraEffects_Names.SPELL_AURA_SPLIT_DAMAGE_PCT, AuraEffects_Names.SPELL_AURA_POWER_BURN_MANA
                        Return True
                    Case AuraEffects_Names.SPELL_AURA_MOD_DAMAGE_DONE, AuraEffects_Names.SPELL_AURA_MOD_STAT, AuraEffects_Names.SPELL_AURA_MOD_PERCENT_STAT, AuraEffects_Names.SPELL_AURA_MOD_TOTAL_STAT_PERCENTAGE
                        If valueBase < 0 Then Return True Else Return False
                    Case Else
                        Return False
                End Select
            End Get
        End Property
    End Class
    Public Class SpellTargets
        Public unitTarget As BaseUnit = Nothing
        Public goTarget As BaseObject = Nothing
        Public itemTarget As ItemObject = Nothing
        Public srcX As Single = 0
        Public srcY As Single = 0
        Public srcZ As Single = 0
        Public dstX As Single = 0
        Public dstY As Single = 0
        Public dstZ As Single = 0
        Public stringTarget As String = ""

        Public targetMask As Integer = 0
        Public targetMaskExtended As Integer = 0

        Public Sub ReadTargets(ByRef packet As PacketClass, ByRef Caster As BaseObject)
            targetMask = packet.GetInt16
            targetMaskExtended = packet.GetInt16

            If targetMask = SpellCastTargetFlags.TARGET_FLAG_SELF Then
                unitTarget = Caster
                Exit Sub
            End If

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_UNIT) = SpellCastTargetFlags.TARGET_FLAG_UNIT Then
                Dim GUID As ULong = packet.GetPackGUID
                If GuidIsCreature(GUID) Then
                    unitTarget = WORLD_CREATUREs(GUID)
                ElseIf GuidIsPlayer(GUID) Then
                    unitTarget = CHARACTERs(GUID)
                End If
#If DEBUG Then
                Log.WriteLine(LogType.DEBUG, "DEBUG: TARGET_FLAG_UNIT {0:X}", GUID)
#End If
            End If

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_OBJECT) = SpellCastTargetFlags.TARGET_FLAG_OBJECT Then
                Dim GUID As ULong = packet.GetPackGUID
                If GuidIsGameObject(GUID) Then
                    goTarget = WORLD_GAMEOBJECTs(GUID)
                ElseIf GuidIsCorpse(GUID) Then
                    goTarget = WORLD_CORPSEOBJECTs(GUID)
                End If
#If DEBUG Then
                Log.WriteLine(LogType.DEBUG, "DEBUG: TARGET_FLAG_OBJECT {0:X}", GUID)
#End If
            End If

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_ITEM) Then
                Dim GUID As ULong = packet.GetUInt64
                itemTarget = WORLD_ITEMs(GUID)
#If DEBUG Then
                Log.WriteLine(LogType.DEBUG, "DEBUG: TARGET_FLAG_ITEM {0:X}", GUID)
#End If
            End If

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_SOURCE_LOCATION) Then
                srcX = packet.GetFloat
                srcY = packet.GetFloat
                srcZ = packet.GetFloat
#If DEBUG Then
                Log.WriteLine(LogType.DEBUG, "DEBUG: TARGET_FLAG_SOURCE_LOCATION {0} {1} {2}", srcX, srcY, srcZ)
#End If
            End If

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_DEST_LOCATION) Then
                dstX = packet.GetFloat
                dstY = packet.GetFloat
                dstZ = packet.GetFloat
#If DEBUG Then
                Log.WriteLine(LogType.DEBUG, "DEBUG: TARGET_FLAG_DEST_LOCATION {0} {1} {2}", dstX, dstY, dstZ)
#End If
            End If

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_STRING) = SpellCastTargetFlags.TARGET_FLAG_STRING Then stringTarget = packet.GetString
        End Sub
        Public Sub WriteTargets(ByRef packet As PacketClass)
            'NOTE: Packed GUIDs are used only on server packets ?

            packet.AddInt16(targetMask)
            packet.AddInt16(targetMaskExtended)

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_UNIT) = SpellCastTargetFlags.TARGET_FLAG_UNIT Then packet.AddUInt64(unitTarget.GUID) 'WritePacketGUID(packet, unitTarget.GUID)
            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_OBJECT) = SpellCastTargetFlags.TARGET_FLAG_OBJECT Then packet.AddUInt64(goTarget.GUID) 'WritePacketGUID(packet, goTarget.GUID)
            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_ITEM) = SpellCastTargetFlags.TARGET_FLAG_ITEM Then packet.AddUInt64(itemTarget.GUID) 'WritePacketGUID(packet, unitTarget.GUID)

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_SOURCE_LOCATION) = SpellCastTargetFlags.TARGET_FLAG_SOURCE_LOCATION Then
                packet.AddSingle(srcX)
                packet.AddSingle(srcY)
                packet.AddSingle(srcZ)
            End If

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_DEST_LOCATION) = SpellCastTargetFlags.TARGET_FLAG_DEST_LOCATION Then
                packet.AddSingle(dstX)
                packet.AddSingle(dstY)
                packet.AddSingle(dstZ)
            End If

            If (targetMask And SpellCastTargetFlags.TARGET_FLAG_STRING) = SpellCastTargetFlags.TARGET_FLAG_STRING Then packet.AddString(stringTarget)
        End Sub

        Public Sub SetTarget_SELF(ByRef c As BaseUnit)
            unitTarget = c
            targetMask += SpellCastTargetFlags.TARGET_FLAG_SELF
        End Sub
        Public Sub SetTarget_UNIT(ByRef c As BaseUnit)
            unitTarget = c
            targetMask += SpellCastTargetFlags.TARGET_FLAG_UNIT
        End Sub
        Public Sub SetTarget_OBJECT(ByRef o As BaseObject)
            Me.goTarget = o
            targetMask += SpellCastTargetFlags.TARGET_FLAG_OBJECT
        End Sub
        Public Sub SetTarget_ITEM(ByRef i As ItemObject)
            Me.itemTarget = i
            targetMask += SpellCastTargetFlags.TARGET_FLAG_ITEM
        End Sub
        Public Sub SetTarget_SOURCELOCATION(ByVal x As Single, ByVal y As Single, ByVal z As Single)
            Me.srcX = x
            Me.srcY = y
            Me.srcZ = z
            targetMask += SpellCastTargetFlags.TARGET_FLAG_SOURCE_LOCATION
        End Sub
        Public Sub SetTarget_DESTINATIONLOCATION(ByVal x As Single, ByVal y As Single, ByVal z As Single)
            Me.dstX = x
            Me.dstY = y
            Me.dstZ = z
            targetMask += SpellCastTargetFlags.TARGET_FLAG_DEST_LOCATION
        End Sub
        Public Sub SetTarget_STRING(ByVal str As String)
            Me.stringTarget = str
            targetMask += SpellCastTargetFlags.TARGET_FLAG_STRING
        End Sub
    End Class

    Public Class CastSpellParameters
        Public tmpTargets As SpellTargets
        Public tmpCaster As BaseObject
        Public tmpSpellID As Integer
        Public tmpCN As Byte
        Public tmpItem As ItemObject = Nothing
        Public tmpInstant As Boolean = False

        Public Sub Cast(ByVal status As Object)
            CType(SPELLs(tmpSpellID), SpellInfo).Cast(tmpCaster, tmpTargets, tmpCN, tmpItem, tmpInstant)
        End Sub
    End Class

    Public Sub SendCastResult(ByVal result As SpellFailedReason, ByRef Client As ClientClass, ByVal id As Integer, ByVal Extra_Cast_Number As Byte)
        If result = SpellFailedReason.CAST_NO_ERROR Then Exit Sub
        Dim packet As New PacketClass(OPCODES.SMSG_CAST_FAILED)
        packet.AddInt32(id)
        packet.AddInt8(result)
        packet.AddInt8(Extra_Cast_Number)

        Select Case result
            Case SpellFailedReason.CAST_FAIL_REQUIRES_SPELL_FOCUS
                packet.AddInt32(CType(SPELLs(id), SpellInfo).RequredCasterStance)
            Case SpellFailedReason.CAST_FAIL_REQUIRES_AREA
                If id = 41618 OrElse id = 41620 Then
                    packet.AddInt32(3842)
                ElseIf id = 41617 OrElse 41619 Then
                    packet.AddInt32(3842)
                Else
                    packet.AddInt32(CType(SPELLs(id), SpellInfo).RequiredAreaID)
                End If
            Case SpellFailedReason.CAST_FAIL_TOTEMS
                If CType(SPELLs(id), SpellInfo).Totem(0) Then packet.AddInt32(CType(SPELLs(id), SpellInfo).Totem(0))
                If CType(SPELLs(id), SpellInfo).Totem(1) Then packet.AddInt32(CType(SPELLs(id), SpellInfo).Totem(1))
            Case SpellFailedReason.CAST_FAIL_TOTEM_CATEGORY
                If CType(SPELLs(id), SpellInfo).TotemCategory(0) Then packet.AddInt32(CType(SPELLs(id), SpellInfo).TotemCategory(0))
                If CType(SPELLs(id), SpellInfo).TotemCategory(1) Then packet.AddInt32(CType(SPELLs(id), SpellInfo).TotemCategory(1))
            Case SpellFailedReason.CAST_FAIL_EQUIPPED_ITEM_CLASS
                packet.AddInt32(CType(SPELLs(id), SpellInfo).EquippedItemClass)
                packet.AddInt32(CType(SPELLs(id), SpellInfo).EquippedItemSubClass)
                packet.AddInt32(CType(SPELLs(id), SpellInfo).EquippedItemInventoryType)
        End Select

        Client.Send(packet)
        packet.Dispose()
    End Sub
    Public Sub SendNonMeleeDamageLog(ByRef Caster As BaseUnit, ByRef Target As BaseUnit, ByVal SpellID As Integer, ByVal Damage As Integer, ByVal Resist As Integer, ByVal Absorbed As Integer, ByVal CriticalHit As Boolean)
        Dim packet As New PacketClass(OPCODES.SMSG_SPELLNONMELEEDAMAGELOG)
        packet.AddPackGUID(Target.GUID)
        packet.AddPackGUID(Caster.GUID)
        packet.AddInt32(SpellID)
        packet.AddInt32(Damage)
        packet.AddInt8(CType(SPELLs(SpellID), SpellInfo).School)
        packet.AddInt32(Absorbed)       'AbsorbedDamage
        packet.AddInt32(Resist)         'Resist
        packet.AddInt8(0)               '1=Suffers/0=Hit
        packet.AddInt8(0)               'Unk
        packet.AddInt32(0)              'Blocked
        If CriticalHit Then
            packet.AddInt32(&H27)
        Else
            packet.AddInt32(&H25)
        End If
        packet.AddInt8(0)               'Unk
        If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
        Caster.SendToNearPlayers(packet)
    End Sub
    Public Sub SendHealSpellLog(ByRef Caster As BaseUnit, ByRef Target As BaseUnit, ByVal SpellID As Integer, ByVal Damage As Integer, ByVal CriticalHit As Boolean)
        Dim packet As New PacketClass(OPCODES.SMSG_SPELLHEALLOG)
        packet.AddPackGUID(Target.GUID)
        packet.AddPackGUID(Caster.GUID)
        packet.AddInt32(SpellID)
        packet.AddInt32(Damage)
        If CriticalHit Then
            packet.AddInt8(1)
        Else
            packet.AddInt8(0)
        End If
        packet.AddInt8(0) 'Unknown
        If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
        Caster.SendToNearPlayers(packet)
    End Sub
    Public Sub SendEnergizeSpellLog(ByRef Caster As BaseUnit, ByRef Target As BaseUnit, ByVal SpellID As Integer, ByVal Damage As Integer, ByVal PowerType As Integer)
        Dim packet As New PacketClass(OPCODES.SMSG_SPELLENERGIZELOG)
        packet.AddPackGUID(Target.GUID)
        packet.AddPackGUID(Caster.GUID)
        packet.AddInt32(SpellID)
        packet.AddInt32(PowerType)
        packet.AddInt32(Damage)
        If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
        Caster.SendToNearPlayers(packet)
    End Sub
    Public Sub SendPeriodicAuraLog(ByRef Caster As BaseUnit, ByRef Target As BaseUnit, ByVal SpellID As Integer, ByVal Damage As Integer, ByVal Absorb As Integer, ByVal Resist As Integer, ByVal AuraIndex As Integer)
        Dim packet As New PacketClass(OPCODES.SMSG_PERIODICAURALOG)
        packet.AddPackGUID(Target.GUID)
        packet.AddPackGUID(Caster.GUID)
        packet.AddInt32(SpellID)
        packet.AddInt32(1)
        packet.AddInt32(AuraIndex)
        packet.AddInt32(Damage)
        packet.AddInt8(CType(SPELLs(SpellID), SpellInfo).School)
        packet.AddInt32(Absorb)
        packet.AddInt32(Resist)

        If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
        Caster.SendToNearPlayers(packet)
    End Sub
    Public Sub SendPeriodicHealAuraLog(ByRef Caster As BaseUnit, ByRef Target As BaseUnit, ByVal SpellID As Integer, ByVal Damage As Integer, ByVal AuraIndex As Integer)
        Dim packet As New PacketClass(OPCODES.SMSG_PERIODICAURALOG)
        packet.AddPackGUID(Target.GUID)
        packet.AddPackGUID(Caster.GUID)
        packet.AddInt32(SpellID)
        packet.AddInt32(1)
        packet.AddInt32(AuraIndex)
        packet.AddInt32(Damage)

        If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
        Caster.SendToNearPlayers(packet)
    End Sub
    Public Sub SendPeriodicEnergizeAuraLog(ByRef Caster As BaseUnit, ByRef Target As BaseUnit, ByVal SpellID As Integer, ByVal Power As ManaTypes, ByVal Damage As Integer, ByVal AuraIndex As Integer)
        Dim packet As New PacketClass(OPCODES.SMSG_PERIODICAURALOG)
        packet.AddPackGUID(Target.GUID)
        packet.AddPackGUID(Caster.GUID)
        packet.AddInt32(SpellID)
        packet.AddInt32(1)
        packet.AddInt32(AuraIndex)
        packet.AddInt32(Power)
        packet.AddInt32(Damage)

        If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
        Caster.SendToNearPlayers(packet)
    End Sub
    Public Sub SendPeriodicManaLeechAuraLog(ByRef Caster As BaseUnit, ByRef Target As BaseUnit, ByVal SpellID As Integer, ByVal Power As ManaTypes, ByVal Damage As Integer, ByVal Multiplier As Single, ByVal AuraIndex As Integer)
        Dim packet As New PacketClass(OPCODES.SMSG_PERIODICAURALOG)
        packet.AddPackGUID(Target.GUID)
        packet.AddPackGUID(Caster.GUID)
        packet.AddInt32(SpellID)
        packet.AddInt32(1)
        packet.AddInt32(AuraIndex)
        packet.AddInt32(Power)
        packet.AddInt32(Damage)
        packet.AddSingle(Multiplier)

        If TypeOf Caster Is CharacterObject Then CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
        Caster.SendToNearPlayers(packet)
    End Sub
    Public Sub SendPlaySpellVisual(ByRef Caster As BaseUnit, ByVal SpellId As Integer)
        Dim packet As New PacketClass(OPCODES.SMSG_PLAY_SPELL_VISUAL)
        packet.AddUInt64(Caster.GUID)
        packet.AddInt32(SpellId)
        Caster.SendToNearPlayers(packet)
        packet.Dispose()
    End Sub

#End Region
#Region "WS.Spells.Database"

    Public SPELLs As New Dictionary(Of Integer, SpellInfo)(29000)

    Public SpellCastTime As New Dictionary(Of Integer, Integer)
    Public SpellRadius As New Dictionary(Of Integer, Single)
    Public SpellRange As New Dictionary(Of Integer, Single)
    Public SpellDuration As New Dictionary(Of Integer, Integer)
    Public SpellFocusObject As New Dictionary(Of Integer, String)

    Public Sub InitializeSpellDB()
        Dim i As Integer
        For i = 0 To SPELL_EFFECT_COUNT
            SPELL_EFFECTs(i) = AddressOf SPELL_EFFECT_NOTHING
        Next

        SPELL_EFFECTs(0) = AddressOf SPELL_EFFECT_NOTHING                   'None
        SPELL_EFFECTs(1) = AddressOf SPELL_EFFECT_INSTAKILL                 'Instakill
        SPELL_EFFECTs(2) = AddressOf SPELL_EFFECT_SCHOOL_DAMAGE             'School Damage
        SPELL_EFFECTs(3) = AddressOf SPELL_EFFECT_DUMMY                     'Dummy
        'SPELL_EFFECTs(4) = AddressOf SPELL_EFFECT_PORTAL_TELEPORT           'Portal Teleport
        SPELL_EFFECTs(5) = AddressOf SPELL_EFFECT_TELEPORT_UNITS            'Teleport Units
        SPELL_EFFECTs(6) = AddressOf SPELL_EFFECT_APPLY_AURA                'Apply Aura
        SPELL_EFFECTs(7) = AddressOf SPELL_EFFECT_ENVIRONMENTAL_DAMAGE      'Environmental Damage
        SPELL_EFFECTs(8) = AddressOf SPELL_EFFECT_MANA_DRAIN                'Power Drain
        'SPELL_EFFECTs(9) = AddressOf SPELL_EFFECT_HEALTH_LEECH              'Health Leech
        SPELL_EFFECTs(10) = AddressOf SPELL_EFFECT_HEAL                     'Heal
        SPELL_EFFECTs(11) = AddressOf SPELL_EFFECT_BIND                     'Bind
        'SPELL_EFFECTs(12) = AddressOf SPELL_EFFECT_PORTAL                   'Portal
        'SPELL_EFFECTs(13) = AddressOf SPELL_EFFECT_RITUAL_BASE              'Ritual Base
        'SPELL_EFFECTs(14) = AddressOf SPELL_EFFECT_RITUAL_SPECIALIZE        'Ritual Specialize
        'SPELL_EFFECTs(15) = AddressOf SPELL_EFFECT_RITUAL_ACTIVATE_PORTAL   'Ritual Activate Portal
        SPELL_EFFECTs(16) = AddressOf SPELL_EFFECT_QUEST_COMPLETE           'Quest Complete
        SPELL_EFFECTs(17) = AddressOf SPELL_EFFECT_WEAPON_DAMAGE_NOSCHOOL   'Weapon Damage + (noschool)
        SPELL_EFFECTs(18) = AddressOf SPELL_EFFECT_RESURRECT                'Resurrect
        '!! SPELL_EFFECTs(19) = AddressOf SPELL_EFFECT_ADD_EXTRA_ATTACKS        'Extra Attacks
        SPELL_EFFECTs(20) = AddressOf SPELL_EFFECT_DODGE                    'Dodge
        SPELL_EFFECTs(21) = AddressOf SPELL_EFFECT_EVADE                    'Evade
        SPELL_EFFECTs(22) = AddressOf SPELL_EFFECT_PARRY                    'Parry
        SPELL_EFFECTs(23) = AddressOf SPELL_EFFECT_BLOCK                    'Block
        SPELL_EFFECTs(24) = AddressOf SPELL_EFFECT_CREATE_ITEM              'Create Item
        'SPELL_EFFECTs(25) = AddressOf SPELL_EFFECT_WEAPON                   'Weapon
        'SPELL_EFFECTs(26) = AddressOf SPELL_EFFECT_DEFENSE                  'Defense
        SPELL_EFFECTs(27) = AddressOf SPELL_EFFECT_PERSISTENT_AREA_AURA     'Persistent Area Aura
        SPELL_EFFECTs(28) = AddressOf SPELL_EFFECT_SUMMON                   'Summon
        SPELL_EFFECTs(29) = AddressOf SPELL_EFFECT_LEAP                     'Leap
        SPELL_EFFECTs(30) = AddressOf SPELL_EFFECT_ENERGIZE                 'Energize
        'SPELL_EFFECTs(31) = AddressOf SPELL_EFFECT_WEAPON_PERCENT_DAMAGE    'Weapon % Dmg
        'SPELL_EFFECTs(32) = AddressOf SPELL_EFFECT_TRIGGER_MISSILE          'Trigger Missile
        SPELL_EFFECTs(33) = AddressOf SPELL_EFFECT_OPEN_LOCK                'Open Lock
        'SPELL_EFFECTs(34) = AddressOf SPELL_EFFECT_SUMMON_MOUNT_OBSOLETE
        SPELL_EFFECTs(35) = AddressOf SPELL_EFFECT_APPLY_AREA_AURA          'Apply Area Aura
        SPELL_EFFECTs(36) = AddressOf SPELL_EFFECT_LEARN_SPELL              'Learn Spell
        'SPELL_EFFECTs(37) = AddressOf SPELL_EFFECT_SPELL_DEFENSE            'Spell Defense
        '! SPELL_EFFECTs(38) = AddressOf SPELL_EFFECT_DISPEL                   'Dispel
        'SPELL_EFFECTs(39) = AddressOf SPELL_EFFECT_LANGUAGE                 'Language
        SPELL_EFFECTs(40) = AddressOf SPELL_EFFECT_DUAL_WIELD               'Dual Wield
        '!! SPELL_EFFECTs(41) = AddressOf SPELL_EFFECT_SUMMON_WILD              'Summon Wild
        '!! SPELL_EFFECTs(42) = AddressOf SPELL_EFFECT_SUMMON_GUARDIAN          'Summon Guardian
        '! SPELL_EFFECTs(43) = AddressOf SPELL_EFFECT_TELEPORT_UNITS_FACE_CASTER
        SPELL_EFFECTs(44) = AddressOf SPELL_EFFECT_SCHOOL_DAMAGE            'Skill Step
        SPELL_EFFECTs(45) = AddressOf SPELL_EFFECT_HONOR
        'SPELL_EFFECTs(46) = AddressOf SPELL_EFFECT_SPAWN                    'Spawn
        'SPELL_EFFECTs(47) = AddressOf SPELL_EFFECT_TRADE_SKILL              'Spell Cast UI
        SPELL_EFFECTs(48) = AddressOf SPELL_EFFECT_STEALTH                  'Stealth
        SPELL_EFFECTs(49) = AddressOf SPELL_EFFECT_DETECT                   'Detect
        SPELL_EFFECTs(50) = AddressOf SPELL_EFFECT_SUMMON_OBJECT            'Summon Object
        'SPELL_EFFECTs(51) = AddressOf SPELL_EFFECT_FORCE_CRITICAL_HIT       'Force Critical Hit
        'SPELL_EFFECTs(52) = AddressOf SPELL_EFFECT_GUARANTEE_HIT            'Guarantee Hit
        'SPELL_EFFECTs(53) = AddressOf SPELL_EFFECT_ENCHANT_ITEM             'Enchant Item Permanent
        'SPELL_EFFECTs(54) = AddressOf SPELL_EFFECT_ENCHANT_ITEM_TEMPORARY   'Enchant Item Temporary
        'SPELL_EFFECTs(55) = AddressOf SPELL_EFFECT_TAMECREATURE             'Tame Creature
        'SPELL_EFFECTs(56) = AddressOf SPELL_EFFECT_SUMMON_PET               'Summon Pet
        'SPELL_EFFECTs(57) = AddressOf SPELL_EFFECT_LEARN_PET_SPELL          'Learn Pet Spell
        'SPELL_EFFECTs(58) = AddressOf SPELL_EFFECT_WEAPON_DAMAGE            'Weapon Damage +
        'SPELL_EFFECTs(59) = AddressOf SPELL_EFFECT_OPEN_LOCK_ITEM           'Open Lock (Item)
        'SPELL_EFFECTs(60) = AddressOf SPELL_EFFECT_PROFICIENCY              'Proficiency
        'SPELL_EFFECTs(61) = AddressOf SPELL_EFFECT_SEND_EVENT               'Send Event
        'SPELL_EFFECTs(62) = AddressOf SPELL_EFFECT_POWER_BURN               'Power Burn
        'SPELL_EFFECTs(63) = AddressOf SPELL_EFFECT_THREAT                   'Threat
        SPELL_EFFECTs(64) = AddressOf SPELL_EFFECT_TRIGGER_SPELL            'Trigger Spell
        'SPELL_EFFECTs(65) = AddressOf SPELL_EFFECT_HEALTH_FUNNEL            'Health Funnel
        'SPELL_EFFECTs(66) = AddressOf SPELL_EFFECT_POWER_FUNNEL             'Power Funnel
        SPELL_EFFECTs(67) = AddressOf SPELL_EFFECT_HEAL_MAX_HEALTH          'Heal Max Health
        SPELL_EFFECTs(68) = AddressOf SPELL_EFFECT_INTERRUPT_CAST           'Interrupt Cast
        'SPELL_EFFECTs(69) = AddressOf SPELL_EFFECT_DISTRACT                 'Distract
        'SPELL_EFFECTs(70) = AddressOf SPELL_EFFECT_PULL                     'Pull
        'SPELL_EFFECTs(71) = AddressOf SPELL_EFFECT_PICKPOCKET               'Pickpocket
        'SPELL_EFFECTs(72) = AddressOf SPELL_EFFECT_ADD_FARSIGHT             'Add Farsight
        'SPELL_EFFECTs(73) = AddressOf SPELL_EFFECT_SUMMON_POSSESSED         'Summon Possessed
        'SPELL_EFFECTs(74) = AddressOf SPELL_EFFECT_SUMMON_TOTEM             'Summon Totem
        'SPELL_EFFECTs(75) = AddressOf SPELL_EFFECT_HEAL_MECHANICAL          'Heal Mechanical
        'SPELL_EFFECTs(76) = AddressOf SPELL_EFFECT_SUMMON_OBJECT_WILD       'Summon Object (Wild)
        'SPELL_EFFECTs(77) = AddressOf SPELL_EFFECT_SCRIPT_EFFECT            'Script Effect
        'SPELL_EFFECTs(78) = AddressOf SPELL_EFFECT_ATTACK                   'Attack
        'SPELL_EFFECTs(79) = AddressOf SPELL_EFFECT_SANCTUARY                'Sanctuary
        'SPELL_EFFECTs(80) = AddressOf SPELL_EFFECT_ADD_COMBO_POINTS         'Add Combo Points
        'SPELL_EFFECTs(81) = AddressOf SPELL_EFFECT_CREATE_HOUSE             'Create House
        'SPELL_EFFECTs(82) = AddressOf SPELL_EFFECT_BIND_SIGHT               'Bind Sight
        SPELL_EFFECTs(83) = AddressOf SPELL_EFFECT_DUEL                     'Duel
        'SPELL_EFFECTs(84) = AddressOf SPELL_EFFECT_STUCK                    'Stuck
        'SPELL_EFFECTs(85) = AddressOf SPELL_EFFECT_SUMMON_PLAYER            'Summon Player
        'SPELL_EFFECTs(86) = AddressOf SPELL_EFFECT_ACTIVATE_OBJECT          'Activate Object
        'SPELL_EFFECTs(87) = AddressOf SPELL_EFFECT_SUMMON_TOTEM_SLOT1       'Summon Totem (slot 1)
        'SPELL_EFFECTs(88) = AddressOf SPELL_EFFECT_SUMMON_TOTEM_SLOT2       'Summon Totem (slot 2)
        'SPELL_EFFECTs(89) = AddressOf SPELL_EFFECT_SUMMON_TOTEM_SLOT3       'Summon Totem (slot 3)
        'SPELL_EFFECTs(90) = AddressOf SPELL_EFFECT_SUMMON_TOTEM_SLOT4       'Summon Totem (slot 4)
        'SPELL_EFFECTs(91) = AddressOf SPELL_EFFECT_THREAT_ALL               'Threat (All)
        'SPELL_EFFECTs(92) = AddressOf SPELL_EFFECT_ENCHANT_HELD_ITEM        'Enchant Held Item
        'SPELL_EFFECTs(93) = AddressOf SPELL_EFFECT_SUMMON_PHANTASM          'Summon Phantasm
        'SPELL_EFFECTs(94) = AddressOf SPELL_EFFECT_SELF_RESURRECT           'Self Resurrect
        'SPELL_EFFECTs(95) = AddressOf SPELL_EFFECT_SKINNING                 'Skinning
        'SPELL_EFFECTs(96) = AddressOf SPELL_EFFECT_CHARGE                   'Charge
        'SPELL_EFFECTs(97) = AddressOf SPELL_EFFECT_SUMMON_CRITTER           'Summon Critter
        'SPELL_EFFECTs(98) = AddressOf SPELL_EFFECT_KNOCK_BACK               'Knock Back
        'SPELL_EFFECTs(99) = AddressOf SPELL_EFFECT_DISENCHANT               'Disenchant
        'SPELL_EFFECTs(100) = AddressOf SPELL_EFFECT_INEBRIATE               'Inebriate
        'SPELL_EFFECTs(101) = AddressOf SPELL_EFFECT_FEED_PET                'Feed Pet
        'SPELL_EFFECTs(102) = AddressOf SPELL_EFFECT_DISMISS_PET             'Dismiss Pet
        'SPELL_EFFECTs(103) = AddressOf SPELL_EFFECT_REPUTATION              'Reputation
        'SPELL_EFFECTs(104) = AddressOf SPELL_EFFECT_SUMMON_OBJECT_SLOT1     'Summon Object (slot 1)
        'SPELL_EFFECTs(105) = AddressOf SPELL_EFFECT_SUMMON_OBJECT_SLOT2     'Summon Object (slot 2)
        'SPELL_EFFECTs(106) = AddressOf SPELL_EFFECT_SUMMON_OBJECT_SLOT3     'Summon Object (slot 3)
        'SPELL_EFFECTs(107) = AddressOf SPELL_EFFECT_SUMMON_OBJECT_SLOT4     'Summon Object (slot 4)
        'SPELL_EFFECTs(108) = AddressOf SPELL_EFFECT_DISPEL_MECHANIC         'Dispel Mechanic
        'SPELL_EFFECTs(109) = AddressOf SPELL_EFFECT_SUMMON_DEAD_PET         'Summon Dead Pet
        'SPELL_EFFECTs(110) = AddressOf SPELL_EFFECT_DESTROY_ALL_TOTEMS      'Destroy All Totems
        'SPELL_EFFECTs(111) = AddressOf SPELL_EFFECT_DURABILITY_DAMAGE       'Durability Damage
        'SPELL_EFFECTs(112) = AddressOf SPELL_EFFECT_SUMMON_DEMON            'Summon Demon
        'SPELL_EFFECTs(113) = AddressOf SPELL_EFFECT_RESURRECT_NEW           'Resurrect (Flat)
        'SPELL_EFFECTs(114) = AddressOf SPELL_EFFECT_ATTACK_ME               'Attack Me

        'SPELL_EFFECTs(115) = AddressOf SPELL_EFFECT_DURABILITY_DAMAGE_PCT
        'SPELL_EFFECTs(116) = AddressOf SPELL_EFFECT_SKIN_PLAYER_CORPSE
        'SPELL_EFFECTs(117) = AddressOf SPELL_EFFECT_SPIRIT_HEAL
        'SPELL_EFFECTs(118) = AddressOf SPELL_EFFECT_SKILL
        'SPELL_EFFECTs(119) = AddressOf SPELL_EFFECT_APPLY_AURA_NEW
        SPELL_EFFECTs(120) = AddressOf SPELL_EFFECT_TELEPORT_GRAVEYARD
        'SPELL_EFFECTs(121) = AddressOf SPELL_EFFECT_ADICIONAL_DMG

        'SPELL_EFFECTs(122) = AddressOf SPELL_EFFECT_?
        'SPELL_EFFECTs(123) = AddressOf SPELL_EFFECT_TAXI                   'Taxi Flight
        'SPELL_EFFECTs(124) = AddressOf SPELL_EFFECT_PULL_TOWARD            'Pull target towards you
        'SPELL_EFFECTs(125) = AddressOf SPELL_EFFECT_INVISIBILITY_NEW       '
        'SPELL_EFFECTs(126) = AddressOf SPELL_EFFECT_SPELL_STEAL            'Steal benefical effect
        'SPELL_EFFECTs(127) = AddressOf SPELL_EFFECT_PROSPECT               'Search ore for gems
        'SPELL_EFFECTs(128) = AddressOf SPELL_EFFECT_APPLY_AURA_NEW2
        'SPELL_EFFECTs(129) = AddressOf SPELL_EFFECT_APPLY_AURA_NEW3
        'SPELL_EFFECTs(130) = AddressOf SPELL_EFFECT_REDIRECT_THREAT
        'SPELL_EFFECTs(131) = AddressOf SPELL_EFFECT_?
        'SPELL_EFFECTs(132) = AddressOf SPELL_EFFECT_?
        'SPELL_EFFECTs(133) = AddressOf SPELL_EFFECT_FORGET
        'SPELL_EFFECTs(134) = AddressOf SPELL_EFFECT_KILL_CREDIT
        'SPELL_EFFECTs(135) = AddressOf SPELL_EFFECT_SUMMON_PET_NEW

        For i = 0 To AURAs_COUNT
            AURAs(i) = AddressOf SPELL_AURA_NONE
        Next

        AURAs(0) = AddressOf SPELL_AURA_NONE                                            'None
        AURAs(1) = AddressOf SPELL_AURA_BIND_SIGHT                                      'Bind Sight
        AURAs(2) = AddressOf SPELL_AURA_MOD_POSSESS                                     'Mod Possess
        AURAs(3) = AddressOf SPELL_AURA_PERIODIC_DAMAGE                                 'Periodic Damage
        AURAs(4) = AddressOf SPELL_AURA_DUMMY                                           'Dummy
        'AURAs(	5	) = AddressOf 	SPELL_AURA_MOD_CONFUSE				                'Mod Confuse
        'AURAs(	6	) = AddressOf 	SPELL_AURA_MOD_CHARM				                'Mod Charm
        'AURAs(	7	) = AddressOf 	SPELL_AURA_MOD_FEAR				                    'Mod Fear
        AURAs(8) = AddressOf SPELL_AURA_PERIODIC_HEAL                                   'Periodic Heal
        'AURAs(	9	) = AddressOf 	SPELL_AURA_MOD_ATTACKSPEED			                'Mod Attack Speed
        AURAs(10) = AddressOf SPELL_AURA_MOD_THREAT                                     'Mod Threat
        AURAs(11) = AddressOf SPELL_AURA_MOD_TAUNT                                      'Taunt
        AURAs(12) = AddressOf SPELL_AURA_MOD_STUN                                       'Stun
        AURAs(13) = AddressOf SPELL_AURA_MOD_DAMAGE_DONE                                'Mod Damage Done
        'AURAs(14) = AddressOf SPELL_AURA_MOD_DAMAGE_TAKEN                              'Mod Damage Taken
        'AURAs(	15	) = AddressOf 	SPELL_AURA_DAMAGE_SHIELD			                'Damage Shield
        AURAs(16) = AddressOf SPELL_AURA_MOD_STEALTH                                    'Mod Stealth
        AURAs(17) = AddressOf SPELL_AURA_MOD_DETECT                                     'Mod Detect
        AURAs(18) = AddressOf SPELL_AURA_MOD_INVISIBILITY                               'Mod Invisibility
        AURAs(19) = AddressOf SPELL_AURA_MOD_INVISIBILITY_DETECTION                     'Mod Invisibility Detection
        AURAs(20) = AddressOf SPELL_AURA_PERIODIC_HEAL_PERCENT                          'Mod Health Regeneration %
        AURAs(21) = AddressOf SPELL_AURA_PERIODIC_ENERGIZE_PERCENT                      'Mod Mana Regeneration %
        AURAs(22) = AddressOf SPELL_AURA_MOD_RESISTANCE                                 'Mod Resistance
        AURAs(23) = AddressOf SPELL_AURA_PERIODIC_TRIGGER_SPELL                         'Periodic Trigger
        AURAs(24) = AddressOf SPELL_AURA_PERIODIC_ENERGIZE                              'Periodic Energize
        AURAs(25) = AddressOf SPELL_AURA_MOD_PACIFY                                     'Pacify
        AURAs(26) = AddressOf SPELL_AURA_MOD_ROOT                                       'Root
        AURAs(27) = AddressOf SPELL_AURA_MOD_SILENCE                                    'Silence
        'AURAs(	28	) = AddressOf 	SPELL_AURA_REFLECT_SPELLS			                'Reflect Spells %
        AURAs(29) = AddressOf SPELL_AURA_MOD_STAT                                       'Mod Stat
        AURAs(30) = AddressOf SPELL_AURA_MOD_SKILL                                      'Mod Skill
        AURAs(31) = AddressOf SPELL_AURA_MOD_INCREASE_SPEED                             'Mod Speed
        AURAs(32) = AddressOf SPELL_AURA_MOD_INCREASE_MOUNTED_SPEED                     'Mod Speed Mounted
        AURAs(33) = AddressOf SPELL_AURA_MOD_DECREASE_SPEED                             'Mod Speed Slow
        AURAs(34) = AddressOf SPELL_AURA_MOD_INCREASE_HEALTH                            'Mod Increase Health
        AURAs(35) = AddressOf SPELL_AURA_MOD_INCREASE_ENERGY                            'Mod Increase Energy
        AURAs(36) = AddressOf SPELL_AURA_MOD_SHAPESHIFT                                 'Shapeshift
        'AURAs(	37	) = AddressOf 	SPELL_AURA_EFFECT_IMMUNITY			                'Immune Effect
        'AURAs(	38	) = AddressOf 	SPELL_AURA_STATE_IMMUNITY			                'Immune State
        'AURAs(	39	) = AddressOf 	SPELL_AURA_SCHOOL_IMMUNITY			                'Immune School
        'AURAs(	40	) = AddressOf 	SPELL_AURA_DAMAGE_IMMUNITY			                'Immune Damage
        'AURAs(	41	) = AddressOf 	SPELL_AURA_DISPEL_IMMUNITY			                'Immune Dispel Type
        AURAs(42) = AddressOf SPELL_AURA_PROC_TRIGGER_SPELL                       'Proc Trigger Spell
        'AURAs(	43	) = AddressOf 	SPELL_AURA_PROC_TRIGGER_DAMAGE		                'Proc Trigger Damage
        AURAs(44) = AddressOf SPELL_AURA_TRACK_CREATURES                                'Track Creatures
        AURAs(45) = AddressOf SPELL_AURA_TRACK_RESOURCES                                'Track Resources
        'AURAs(	46	) = AddressOf 	SPELL_AURA_MOD_PARRY_SKILL			                'Mod Parry Skill
        'AURAs(	47	) = AddressOf 	SPELL_AURA_MOD_PARRY_PERCENT		                'Mod Parry Percent
        'AURAs(	48	) = AddressOf 	SPELL_AURA_MOD_DODGE_SKILL			                'Mod Dodge Skill
        'AURAs(	49	) = AddressOf 	SPELL_AURA_MOD_DODGE_PERCENT		                'Mod Dodge Percent
        'AURAs(	50	) = AddressOf 	SPELL_AURA_MOD_BLOCK_SKILL			                'Mod Block Skill
        'AURAs(	51	) = AddressOf 	SPELL_AURA_MOD_BLOCK_PERCENT		                'Mod Block Percent
        'AURAs(	52	) = AddressOf 	SPELL_AURA_MOD_CRIT_PERCENT			                'Mod Crit Percent
        AURAs(53) = AddressOf SPELL_AURA_PERIODIC_LEECH                                 'Periodic Leech
        'AURAs(	54	) = AddressOf 	SPELL_AURA_MOD_HIT_CHANCE			                'Mod Hit Chance
        'AURAs(	55	) = AddressOf 	SPELL_AURA_MOD_SPELL_HIT_CHANCE		                'Mod Spell Hit Chance
        AURAs(56) = AddressOf SPELL_AURA_TRANSFORM                                      'Transform
        'AURAs(	57	) = AddressOf 	SPELL_AURA_MOD_SPELL_CRIT_CHANCE	                'Mod Spell Crit Chance
        AURAs(58) = AddressOf SPELL_AURA_MOD_INCREASE_SWIM_SPEED                        'Mod Speed Swim
        'AURAs(	59	) = AddressOf 	SPELL_AURA_MOD_DAMAGE_DONE_CREATURE	                'Mod Creature Dmg Done
        'AURAs(	60	) = AddressOf 	SPELL_AURA_MOD_PACIFY_SILENCE		                'Pacify & Silence
        AURAs(61) = AddressOf SPELL_AURA_MOD_SCALE                                      'Mod Scale
        'AURAs(	62	) = AddressOf 	SPELL_AURA_PERIODIC_HEALTH_FUNNEL	                'Periodic Health Funnel
        'AURAs(	63	) = AddressOf 	SPELL_AURA_PERIODIC_MANA_FUNNEL		                'Periodic Mana Funnel
        AURAs(64) = AddressOf SPELL_AURA_PERIODIC_MANA_LEECH                            'Periodic Mana Leech
        'AURAs(	65	) = AddressOf 	SPELL_AURA_MOD_CASTING_SPEED		                'Haste - Spells
        'AURAs(	66	) = AddressOf 	SPELL_AURA_FEIGN_DEATH				                'Feign Death
        'AURAs(	67	) = AddressOf 	SPELL_AURA_MOD_DISARM				                'Disarm
        'AURAs(	68	) = AddressOf 	SPELL_AURA_MOD_STALKED				                'Mod Stalked
        'AURAs(	69	) = AddressOf 	SPELL_AURA_SCHOOL_ABSORB			                'School Absorb
        'AURAs(	70	) = AddressOf 	SPELL_AURA_EXTRA_ATTACKS			                'Extra Attacks
        'AURAs(	71	) = AddressOf 	SPELL_AURA_MOD_SPELL_CRIT_CHANCE_SCHOOL				'Mod School Spell Crit Chance
        'AURAs(	72	) = AddressOf 	SPELL_AURA_MOD_POWER_COST			                'Mod Power Cost
        'AURAs(	73	) = AddressOf 	SPELL_AURA_MOD_POWER_COST_SCHOOL	                'Mod School Power Cost
        'AURAs(	74	) = AddressOf 	SPELL_AURA_REFLECT_SPELLS_SCHOOL	                'Reflect School Spells %
        AURAs(75) = AddressOf SPELL_AURA_MOD_LANGUAGE                                   'Mod Language
        AURAs(76) = AddressOf SPELL_AURA_FAR_SIGHT                                      'Far Sight
        AURAs(77) = AddressOf SPELL_AURA_MECHANIC_IMMUNITY                              'Immune Mechanic
        AURAs(78) = AddressOf SPELL_AURA_MOUNTED                                        'Mounted
        AURAs(79) = AddressOf SPELL_AURA_MOD_DAMAGE_DONE_PCT                            'Mod Dmg %
        AURAs(80) = AddressOf SPELL_AURA_MOD_STAT_PERCENT                               'Mod Stat %
        'AURAs(	81	) = AddressOf 	SPELL_AURA_SPLIT_DAMAGE				                'Split Damage
        AURAs(82) = AddressOf SPELL_AURA_WATER_BREATHING                                'Water Breathing
        AURAs(83) = AddressOf SPELL_AURA_MOD_BASE_RESISTANCE                            'Mod Base Resistance
        AURAs(84) = AddressOf SPELL_AURA_MOD_REGEN                                      'Mod Health Regen
        AURAs(85) = AddressOf SPELL_AURA_MOD_POWER_REGEN                                'Mod Power Regen
        'AURAs(	86	) = AddressOf 	SPELL_AURA_CHANNEL_DEATH_ITEM		                'Create Death Item
        'AURAs(	87	) = AddressOf 	SPELL_AURA_MOD_DAMAGE_TAKEN_PCT			            'Mod Dmg % Taken
        'AURAs(	88	) = AddressOf 	SPELL_AURA_MOD_REGEN				                'Mod Health Regen Percent
        AURAs(89) = AddressOf SPELL_AURA_PERIODIC_DAMAGE_PERCENT                        'Periodic Damage Percent
        'AURAs(	90	) = AddressOf 	SPELL_AURA_MOD_RESIST_CHANCE		                'Mod Resist Chance
        'AURAs(	91	) = AddressOf 	SPELL_AURA_MOD_DETECT_RANGE			                'Mod Detect Range
        'AURAs(	92	) = AddressOf 	SPELL_AURA_PREVENTS_FLEEING			                'Prevent Fleeing
        'AURAs(	93	) = AddressOf 	SPELL_AURA_MOD_UNATTACKABLE			                'Mod Uninteractible
        'AURAs(	94	) = AddressOf 	SPELL_AURA_INTERRUPT_REGEN			                'Interrupt Regen
        AURAs(95) = AddressOf SPELL_AURA_GHOST                                          'Ghost
        'AURAs(	96	) = AddressOf 	SPELL_AURA_SPELL_MAGNET				                'Spell Magnet
        'AURAs(	97	) = AddressOf 	SPELL_AURA_MANA_SHIELD				                'Mana Shield
        'AURAs(	98	) = AddressOf 	SPELL_AURA_MOD_SKILL_TALENT			                'Mod Skill Talent
        AURAs(99) = AddressOf SPELL_AURA_MOD_ATTACK_POWER                               'Mod Attack Power
        'AURAs(	100	) = AddressOf 	SPELL_AURA_AURAS_VISIBLE			                'Auras Visible
        AURAs(101) = AddressOf SPELL_AURA_MOD_RESISTANCE_PCT                            'Mod Resistance %
        'AURAs(	102	) = AddressOf 	SPELL_AURA_MOD_CREATURE_ATTACK_POWER			    'Mod Creature Attack Power
        AURAs(103) = AddressOf SPELL_AURA_MOD_TOTAL_THREAT                              'Mod Total Threat (Fade)
        AURAs(104) = AddressOf SPELL_AURA_WATER_WALK                                    'Water Walk
        AURAs(105) = AddressOf SPELL_AURA_FEATHER_FALL                                  'Feather Fall
        AURAs(106) = AddressOf SPELL_AURA_HOVER                                         'Hover
        'AURAs(	107	) = AddressOf 	SPELL_AURA_ADD_FLAT_MODIFIER		                'Add Flat Modifier
        'AURAs(	108	) = AddressOf 	SPELL_AURA_ADD_PCT_MODIFIER			                'Add % Modifier
        'AURAs(	109	) = AddressOf 	SPELL_AURA_ADD_TARGET_TRIGGER		                'Add Class Target Trigger
        AURAs(110) = AddressOf SPELL_AURA_MOD_POWER_REGEN_PERCENT                       'Mod Power Regen %
        'AURAs(	111	) = AddressOf 	SPELL_AURA_ADD_CASTER_HIT_TRIGGER	                'Add Class Caster Hit Trigger
        'AURAs(	112	) = AddressOf 	SPELL_AURA_OVERRIDE_CLASS_SCRIPTS	                'Override Class Scripts
        'AURAs(	113	) = AddressOf 	SPELL_AURA_MOD_RANGED_DAMAGE_TAKEN	                'Mod Ranged Dmg Taken
        'AURAs(	114	) = AddressOf 	SPELL_AURA_MOD_RANGED_DAMAGE_TAKEN_PCT			    'Mod Ranged % Dmg Taken
        'AURAs(115) = AddressOf SPELL_AURA_MOD_HEALING                                  'Mod Healing
        'AURAs(	116	) = AddressOf 	SPELL_AURA_IGNORE_REGEN_INTERRUPT	                'Regen During Combat
        'AURAs(	117	) = AddressOf 	SPELL_AURA_MOD_MECHANIC_RESISTANCE	                'Mod Mechanic Resistance
        'AURAs(118) = AddressOf SPELL_AURA_MOD_HEALING_PCT                              'Mod Healing %
        'AURAs(	119	) = AddressOf 	SPELL_AURA_SHARE_PET_TRACKING		                'Share Pet Tracking
        'AURAs(	120	) = AddressOf 	SPELL_AURA_UNTRACKABLE				                'Untrackable
        AURAs(121) = AddressOf SPELL_AURA_EMPATHY                                       'Empathy (Lore, whatever)
        'AURAs(	122	) = AddressOf 	SPELL_AURA_MOD_OFFHAND_DAMAGE_PCT	                'Mod Offhand Dmg %
        'AURAs(	123	) = AddressOf 	SPELL_AURA_MOD_POWER_COST_PCT		                'Mod Power Cost %
        AURAs(124) = AddressOf SPELL_AURA_MOD_RANGED_ATTACK_POWER                       'Mod Ranged Attack Power
        'AURAs(	125	) = AddressOf 	SPELL_AURA_MOD_MELEE_DAMAGE_TAKEN	                'Mod Melee Dmg Taken
        'AURAs(	126	) = AddressOf 	SPELL_AURA_MOD_MELEE_DAMAGE_TAKEN_PCT			    'Mod Melee % Dmg Taken
        'AURAs(	127	) = AddressOf 	SPELL_AURA_RANGED_ATTACK_POWER_ATTACKER_BONUS	    'Rngd Atk Pwr Attckr Bonus
        'AURAs(	128	) = AddressOf 	SPELL_AURA_MOD_POSSESS_PET			                'Mod Possess Pet
        AURAs(129) = AddressOf SPELL_AURA_MOD_INCREASE_SPEED_ALWAYS                     'Mod Speed Always
        AURAs(130) = AddressOf SPELL_AURA_MOD_INCREASE_MOUNTED_SPEED_ALWAYS             'Mod Mounted Speed Always
        'AURAs(	131	) = AddressOf 	SPELL_AURA_MOD_CREATURE_RANGED_ATTACK_POWER		    'Mod Creature Ranged Attack Power
        'AURAs(	132	) = AddressOf 	SPELL_AURA_MOD_INCREASE_ENERGY_PERCENT			    'Mod Increase Energy %
        'AURAs(	133	) = AddressOf 	SPELL_AURA_MOD_INCREASE_HEALTH_PERCENT			    'Mod Max Health %
        'AURAs(	134	) = AddressOf 	SPELL_AURA_MOD_MANA_REGEN_INTERRUPT				    'Mod Interrupted Mana Regen
        AURAs(135) = AddressOf SPELL_AURA_MOD_HEALING_DONE                              'Mod Healing Done
        AURAs(136) = AddressOf SPELL_AURA_MOD_HEALING_DONE_PCT                          'Mod Healing Done %
        AURAs(137) = AddressOf SPELL_AURA_MOD_TOTAL_STAT_PERCENTAGE                     'Mod Total Stat %
        'AURAs(	138	) = AddressOf 	SPELL_AURA_MOD_HASTE				                'Haste - Melee
        'AURAs(	139	) = AddressOf 	SPELL_AURA_FORCE_REACTION			                'Force Reaction
        'AURAs(	140	) = AddressOf 	SPELL_AURA_MOD_RANGED_HASTE			                'Haste - Ranged
        'AURAs(	141	) = AddressOf 	SPELL_AURA_MOD_RANGED_AMMO_HASTE	                'Haste - Ranged (Ammo Only)
        AURAs(142) = AddressOf SPELL_AURA_MOD_BASE_RESISTANCE_PCT                       'Mod Base Resistance %
        AURAs(143) = AddressOf SPELL_AURA_MOD_RESISTANCE_EXCLUSIVE                      'Mod Resistance Exclusive
        AURAs(144) = AddressOf SPELL_AURA_SAFE_FALL                                     'Safe Fall
        'AURAs(	145	) = AddressOf 	SPELL_AURA_CHARISMA				                    'Charisma
        'AURAs(	146	) = AddressOf 	SPELL_AURA_PERSUADED				                'Persuaded
        'AURAs(	147	) = AddressOf 	SPELL_AURA_ADD_CREATURE_IMMUNITY	                'Add Creature Immunity
        'AURAs(	148	) = AddressOf 	SPELL_AURA_RETAIN_COMBO_POINTS		                'Retain Combo Points
        'AURAs(	149	) = AddressOf 	SPELL_AURA_RESIST_PUSHBACK			                'Resist Pushback
        'AURAs(	150	) = AddressOf 	SPELL_AURA_MOD_SHIELD_BLOCK			                'Mod Shield Block %
        'AURAs(	151	) = AddressOf 	SPELL_AURA_TRACK_STEALTHED			                'Track Stealthed
        'AURAs(	152	) = AddressOf 	SPELL_AURA_MOD_DETECTED_RANGE		                'Mod Aggro Range
        'AURAs(	153	) = AddressOf 	SPELL_AURA_SPLIT_DAMAGE_FLAT		                'Split Damage Flat
        'AURAs(	154	) = AddressOf 	SPELL_AURA_MOD_STEALTH_LEVEL		                'Stealth Level Modifier
        'AURAs(	155	) = AddressOf 	SPELL_AURA_MOD_WATER_BREATHING		                'Mod Water Breathing
        'AURAs(	156	) = AddressOf 	SPELL_AURA_MOD_REPUTATION_ADJUST	                'Mod Reputation Gain
        'AURAs(	157	) = AddressOf 	SPELL_AURA_PET_DAMAGE_MULTI			                'Mod Pet Damage

        'AURAs(	158	) = AddressOf                                                       'Mod Shield Block
        'AURAs(	159	) = AddressOf                                                       'Honorless
        'AURAs(	160 ) = AddressOf 	                        			                'Mod Side/Rear PBAE Damage Taken %
        'AURAs(	161 ) = AddressOf 	                        			                'Mod Health Regen In Combat
        'AURAs(	162 ) = AddressOf 	                        			                'Power Burn (Mana)
        'AURAs(	163 ) = AddressOf 	                        			                'Mod Critical Damage
        'AURAs(	164 ) = AddressOf  	                        			                'TEST
        'AURAs(	165 ) = AddressOf  	                        			                '
        'AURAs(	166 ) = AddressOf 	                        			                'Mod Attack Power %
        'AURAs(	168 ) = AddressOf 	                        			                'Increase Damage % (vs. %X)
        'AURAs(	169 ) = AddressOf 	                        			                'Increase Critical % (vs. %X)
        'AURAs(	170 ) = AddressOf  	                        			                '
        'AURAs(	171 ) = AddressOf  	                        			                '
        'AURAs(	172 ) = AddressOf  	                        			                '
        'AURAs(	173 ) = AddressOf  	                        			                '
        'AURAs(	174 ) = AddressOf 	                        			                'Increase Spell Damage by % Spirit (Spells)
        'AURAs(	175 ) = AddressOf 	                        			                'Increase Spell Healing by % Spirit
        'AURAs(	176 ) = AddressOf  	                        			                '
        'AURAs(	177 ) = AddressOf 	                        			                'Area Charm
        'AURAs(	178 ) = AddressOf  	                        			                '
        'AURAs(	179 ) = AddressOf  	                        			                '
        'AURAs(	180	) = AddressOf 	                        			                'Increase Spell Damage (vs. %X)
        'AURAs(	171 ) = AddressOf  	                        			                '
        'AURAs(	182	) = AddressOf 	                        			                'Increase Resist by % of Intellect (%X)
        'AURAs(	183	) = AddressOf 	                        			                'Decrease Critical Threat by % (Spells)
        'AURAs(	184	) = AddressOf                                                       'Mod Melee GetHit Chance
        'AURAs(	185	) = AddressOf                                                       'Mod Ranged GetHit Chance
        'AURAs(	186	) = AddressOf                                                       'Mod Spell GetHit Chance
        'AURAs(	187	) = AddressOf                                                       'Mod Melee Critical GetHit Chance
        'AURAs(	188	) = AddressOf                                                       'Mod Ranged Critical GetHit Chance
        'AURAs(	189	) = AddressOf                                                       'Mod Skill Rating
        'AURAs(	190	) = AddressOf                                                       'Mod Reputation Gain
        'AURAs(	191	) = AddressOf                                                       '
        'AURAs(	192	) = AddressOf                                                       '
        'AURAs(	193	) = AddressOf                                                       '
        'AURAs(	194	) = AddressOf                                                       'Mod Spell Damage by % of Intellect
        'AURAs(	195	) = AddressOf                                                       'Mod Spell Healing by % of Intellect
        'AURAs(	196	) = AddressOf                                                       'Mod Global Cooldowns
        'AURAs(	197	) = AddressOf                                                       'No Critical Damage Taken
        'AURAs(	198	) = AddressOf                                                       'Mod Weapon Skills
        'AURAs(	199	) = AddressOf                                                       'Mod Hit Chance
        'AURAs(	200	) = AddressOf                                                       'Mod Gained XP
        'AURAs(	201	) = AddressOf                                                       'Fly
        'AURAs(	202	) = AddressOf                                                       '
        'AURAs(	203	) = AddressOf                                                       'Mod Melee Critical Damage Taken
        'AURAs(	204	) = AddressOf                                                       'Mod Ranged Critical Damage Taken
        'AURAs(	205	) = AddressOf                                                       '
        'AURAs(	206	) = AddressOf 	                        			                'Mod Fly Speed Always
        AURAs(207) = AddressOf SPELL_AURA_MOD_INCREASE_MOUNTED_FLY_SPEED                'Mod Fly Speed Mounted
        AURAs(208) = AddressOf SPELL_AURA_MOD_INCREASE_FLY_SPEED                        'Mod Fly Speed
        AURAs(209) = AddressOf SPELL_AURA_MOD_INCREASE_MOUNTED_FLY_SPEED_ALWAYS         'Mod Fly Speed Mounted Always
        'AURAs(	210	) = AddressOf 	                                                    '
        'AURAs(	211	) = AddressOf 	                                                    '
        'AURAs(	212	) = AddressOf 	                                                    'Mod Ranged Attack Power by % of Intellect
        'AURAs(	213	) = AddressOf 	                                                    'Mod Rage From Damage
        'AURAs(	214	) = AddressOf 	                                                    '
        'AURAs(	215	) = AddressOf 	                                                    'TEST
        'AURAs(	216	) = AddressOf 	                                                    'Mod Casting Speed
        'AURAs(	217	) = AddressOf 	                                                    '
        'AURAs(	218	) = AddressOf 	                                                    '
        'AURAs(	219	) = AddressOf 	                                                    'Mod Regenerate by % of Intellect
        'AURAs(	220	) = AddressOf 	                                                    'Increase Spell Healing by % Strength
        'AURAs(	221	) = AddressOf 	                                                    '
        'AURAs(	222	) = AddressOf 	                                                    '
        'AURAs(	223	) = AddressOf 	                                                    '
        'AURAs(	224	) = AddressOf 	                                                    '
        'AURAs(	225	) = AddressOf 	                                                    '
        AURAs(226) = AddressOf SPELL_AURA_PERIODIC_DUMMY                                'Periodic dummy
    End Sub

#End Region

#Region "WS.Spells.SpellEffects"
    Public Enum SpellEffects_Names As Integer
        SPELL_EFFECT_NOTHING = 0
        SPELL_EFFECT_INSTAKILL = 1
        SPELL_EFFECT_SCHOOL_DAMAGE = 2
        SPELL_EFFECT_DUMMY = 3
        SPELL_EFFECT_PORTAL_TELEPORT = 4
        SPELL_EFFECT_TELEPORT_UNITS = 5
        SPELL_EFFECT_APPLY_AURA = 6
        SPELL_EFFECT_ENVIRONMENTAL_DAMAGE = 7
        SPELL_EFFECT_MANA_DRAIN = 8
        SPELL_EFFECT_HEALTH_LEECH = 9
        SPELL_EFFECT_HEAL = 10
        SPELL_EFFECT_BIND = 11
        SPELL_EFFECT_PORTAL = 12
        SPELL_EFFECT_RITUAL_BASE = 13
        SPELL_EFFECT_RITUAL_SPECIALIZE = 14
        SPELL_EFFECT_RITUAL_ACTIVATE_PORTAL = 15
        SPELL_EFFECT_QUEST_COMPLETE = 16
        SPELL_EFFECT_WEAPON_DAMAGE_NOSCHOOL = 17
        SPELL_EFFECT_RESURRECT = 18
        SPELL_EFFECT_ADD_EXTRA_ATTACKS = 19
        SPELL_EFFECT_DODGE = 20
        SPELL_EFFECT_EVADE = 21
        SPELL_EFFECT_PARRY = 22
        SPELL_EFFECT_BLOCK = 23
        SPELL_EFFECT_CREATE_ITEM = 24
        SPELL_EFFECT_WEAPON = 25
        SPELL_EFFECT_DEFENSE = 26
        SPELL_EFFECT_PERSISTENT_AREA_AURA = 27
        SPELL_EFFECT_SUMMON = 28
        SPELL_EFFECT_LEAP = 29
        SPELL_EFFECT_ENERGIZE = 30
        SPELL_EFFECT_WEAPON_PERCENT_DAMAGE = 31
        SPELL_EFFECT_TRIGGER_MISSILE = 32
        SPELL_EFFECT_OPEN_LOCK = 33
        SPELL_EFFECT_SUMMON_MOUNT_OBSOLETE = 34
        SPELL_EFFECT_APPLY_AREA_AURA = 35
        SPELL_EFFECT_LEARN_SPELL = 36
        SPELL_EFFECT_SPELL_DEFENSE = 37
        SPELL_EFFECT_DISPEL = 38
        SPELL_EFFECT_LANGUAGE = 39
        SPELL_EFFECT_DUAL_WIELD = 40
        SPELL_EFFECT_SUMMON_WILD = 41
        SPELL_EFFECT_SUMMON_GUARDIAN = 42
        SPELL_EFFECT_TELEPORT_UNITS_FACE_CASTER = 43
        SPELL_EFFECT_SKILL_STEP = 44
        SPELL_EFFECT_UNDEFINED_45 = 45
        SPELL_EFFECT_SPAWN = 46
        SPELL_EFFECT_TRADE_SKILL = 47
        SPELL_EFFECT_STEALTH = 48
        SPELL_EFFECT_DETECT = 49
        SPELL_EFFECT_SUMMON_OBJECT = 50
        SPELL_EFFECT_FORCE_CRITICAL_HIT = 51
        SPELL_EFFECT_GUARANTEE_HIT = 52
        SPELL_EFFECT_ENCHANT_ITEM = 53
        SPELL_EFFECT_ENCHANT_ITEM_TEMPORARY = 54
        SPELL_EFFECT_TAMECREATURE = 55
        SPELL_EFFECT_SUMMON_PET = 56
        SPELL_EFFECT_LEARN_PET_SPELL = 57
        SPELL_EFFECT_WEAPON_DAMAGE = 58
        SPELL_EFFECT_OPEN_LOCK_ITEM = 59
        SPELL_EFFECT_PROFICIENCY = 60
        SPELL_EFFECT_SEND_EVENT = 61
        SPELL_EFFECT_POWER_BURN = 62
        SPELL_EFFECT_THREAT = 63
        SPELL_EFFECT_TRIGGER_SPELL = 64
        SPELL_EFFECT_HEALTH_FUNNEL = 65
        SPELL_EFFECT_POWER_FUNNEL = 66
        SPELL_EFFECT_HEAL_MAX_HEALTH = 67
        SPELL_EFFECT_INTERRUPT_CAST = 68
        SPELL_EFFECT_DISTRACT = 69
        SPELL_EFFECT_PULL = 70
        SPELL_EFFECT_PICKPOCKET = 71
        SPELL_EFFECT_ADD_FARSIGHT = 72
        SPELL_EFFECT_SUMMON_POSSESSED = 73
        SPELL_EFFECT_SUMMON_TOTEM = 74
        SPELL_EFFECT_HEAL_MECHANICAL = 75
        SPELL_EFFECT_SUMMON_OBJECT_WILD = 76
        SPELL_EFFECT_SCRIPT_EFFECT = 77
        SPELL_EFFECT_ATTACK = 78
        SPELL_EFFECT_SANCTUARY = 79
        SPELL_EFFECT_ADD_COMBO_POINTS = 80
        SPELL_EFFECT_CREATE_HOUSE = 81
        SPELL_EFFECT_BIND_SIGHT = 82
        SPELL_EFFECT_DUEL = 83
        SPELL_EFFECT_STUCK = 84
        SPELL_EFFECT_SUMMON_PLAYER = 85
        SPELL_EFFECT_ACTIVATE_OBJECT = 86
        SPELL_EFFECT_SUMMON_TOTEM_SLOT1 = 87
        SPELL_EFFECT_SUMMON_TOTEM_SLOT2 = 88
        SPELL_EFFECT_SUMMON_TOTEM_SLOT3 = 89
        SPELL_EFFECT_SUMMON_TOTEM_SLOT4 = 90
        SPELL_EFFECT_THREAT_ALL = 91
        SPELL_EFFECT_ENCHANT_HELD_ITEM = 92
        SPELL_EFFECT_SUMMON_PHANTASM = 93
        SPELL_EFFECT_SELF_RESURRECT = 94
        SPELL_EFFECT_SKINNING = 95
        SPELL_EFFECT_CHARGE = 96
        SPELL_EFFECT_SUMMON_CRITTER = 97
        SPELL_EFFECT_KNOCK_BACK = 98
        SPELL_EFFECT_DISENCHANT = 99
        SPELL_EFFECT_INEBRIATE = 100
        SPELL_EFFECT_FEED_PET = 101
        SPELL_EFFECT_DISMISS_PET = 102
        SPELL_EFFECT_REPUTATION = 103
        SPELL_EFFECT_SUMMON_OBJECT_SLOT1 = 104
        SPELL_EFFECT_SUMMON_OBJECT_SLOT2 = 105
        SPELL_EFFECT_SUMMON_OBJECT_SLOT3 = 106
        SPELL_EFFECT_SUMMON_OBJECT_SLOT4 = 107
        SPELL_EFFECT_DISPEL_MECHANIC = 108
        SPELL_EFFECT_SUMMON_DEAD_PET = 109
        SPELL_EFFECT_DESTROY_ALL_TOTEMS = 110
        SPELL_EFFECT_DURABILITY_DAMAGE = 111
        SPELL_EFFECT_SUMMON_DEMON = 112
        SPELL_EFFECT_RESURRECT_NEW = 113
        SPELL_EFFECT_ATTACK_ME = 114
        SPELL_EFFECT_DURABILITY_DAMAGE_PCT = 115
        SPELL_EFFECT_SKIN_PLAYER_CORPSE = 116
        SPELL_EFFECT_SPIRIT_HEAL = 117
        SPELL_EFFECT_SKILL = 118
        SPELL_EFFECT_APPLY_AURA_NEW = 119
        SPELL_EFFECT_TELEPORT_GRAVEYARD = 120
        SPELL_EFFECT_ADICIONAL_DMG = 121
        SPELL_EFFECT_122 = 122
        SPELL_EFFECT_123 = 123
        SPELL_EFFECT_PLAYER_PULL = 124
        SPELL_EFFECT_REDUCE_THREAT_PERCENT = 125
        SPELL_EFFECT_STEAL_BENEFICIAL_BUFF = 126
        SPELL_EFFECT_PROSPECTING = 127
        SPELL_EFFECT_APPLY_AREA_AURA_FRIEND = 128
        SPELL_EFFECT_APPLY_AREA_AURA_ENEMY = 129
        SPELL_EFFECT_REDIRECT_THREAT = 130
        SPELL_EFFECT_131 = 131
        SPELL_EFFECT_132 = 132
        SPELL_EFFECT_UNLEARN_SPECIALIZATION = 133
        SPELL_EFFECT_KILL_CREDIT = 134
        SPELL_EFFECT_135 = 135
        SPELL_EFFECT_HEAL_PCT = 136
        SPELL_EFFECT_ENERGIZE_PCT = 137
        SPELL_EFFECT_138 = 138
        SPELL_EFFECT_139 = 139
        SPELL_EFFECT_FORCE_CAST = 140
        SPELL_EFFECT_141 = 141
        SPELL_EFFECT_TRIGGER_SPELL_WITH_VALUE = 142
        SPELL_EFFECT_APPLY_AREA_AURA_OWNER = 143
        SPELL_EFFECT_144 = 144
        SPELL_EFFECT_145 = 145
        SPELL_EFFECT_146 = 146
        SPELL_EFFECT_QUEST_FAIL = 147
        SPELL_EFFECT_148 = 148
        SPELL_EFFECT_149 = 149
        SPELL_EFFECT_150 = 150
        SPELL_EFFECT_TRIGGER_SPELL_2 = 151
        SPELL_EFFECT_152 = 152
        SPELL_EFFECT_153 = 153
    End Enum
    Public Enum AuraEffects_Names As Integer
        SPELL_AURA_NONE = 0
        SPELL_AURA_BIND_SIGHT = 1
        SPELL_AURA_MOD_POSSESS = 2
        SPELL_AURA_PERIODIC_DAMAGE = 3
        SPELL_AURA_DUMMY = 4
        SPELL_AURA_MOD_CONFUSE = 5
        SPELL_AURA_MOD_CHARM = 6
        SPELL_AURA_MOD_FEAR = 7
        SPELL_AURA_PERIODIC_HEAL = 8
        SPELL_AURA_MOD_ATTACKSPEED = 9
        SPELL_AURA_MOD_THREAT = 10
        SPELL_AURA_MOD_TAUNT = 11
        SPELL_AURA_MOD_STUN = 12
        SPELL_AURA_MOD_DAMAGE_DONE = 13
        SPELL_AURA_MOD_DAMAGE_TAKEN = 14
        SPELL_AURA_DAMAGE_SHIELD = 15
        SPELL_AURA_MOD_STEALTH = 16
        SPELL_AURA_MOD_DETECT = 17
        SPELL_AURA_MOD_INVISIBILITY = 18
        SPELL_AURA_MOD_INVISIBILITY_DETECTION = 19
        SPELL_AURA_OBS_MOD_HEALTH = 20                         '2021 unofficial
        SPELL_AURA_OBS_MOD_MANA = 21
        SPELL_AURA_MOD_RESISTANCE = 22
        SPELL_AURA_PERIODIC_TRIGGER_SPELL = 23
        SPELL_AURA_PERIODIC_ENERGIZE = 24
        SPELL_AURA_MOD_PACIFY = 25
        SPELL_AURA_MOD_ROOT = 26
        SPELL_AURA_MOD_SILENCE = 27
        SPELL_AURA_REFLECT_SPELLS = 28
        SPELL_AURA_MOD_STAT = 29
        SPELL_AURA_MOD_SKILL = 30
        SPELL_AURA_MOD_INCREASE_SPEED = 31
        SPELL_AURA_MOD_INCREASE_MOUNTED_SPEED = 32
        SPELL_AURA_MOD_DECREASE_SPEED = 33
        SPELL_AURA_MOD_INCREASE_HEALTH = 34
        SPELL_AURA_MOD_INCREASE_ENERGY = 35
        SPELL_AURA_MOD_SHAPESHIFT = 36
        SPELL_AURA_EFFECT_IMMUNITY = 37
        SPELL_AURA_STATE_IMMUNITY = 38
        SPELL_AURA_SCHOOL_IMMUNITY = 39
        SPELL_AURA_DAMAGE_IMMUNITY = 40
        SPELL_AURA_DISPEL_IMMUNITY = 41
        SPELL_AURA_PROC_TRIGGER_SPELL = 42
        SPELL_AURA_PROC_TRIGGER_DAMAGE = 43
        SPELL_AURA_TRACK_CREATURES = 44
        SPELL_AURA_TRACK_RESOURCES = 45
        SPELL_AURA_MOD_PARRY_SKILL = 46
        SPELL_AURA_MOD_PARRY_PERCENT = 47
        SPELL_AURA_MOD_DODGE_SKILL = 48
        SPELL_AURA_MOD_DODGE_PERCENT = 49
        SPELL_AURA_MOD_BLOCK_SKILL = 50
        SPELL_AURA_MOD_BLOCK_PERCENT = 51
        SPELL_AURA_MOD_CRIT_PERCENT = 52
        SPELL_AURA_PERIODIC_LEECH = 53
        SPELL_AURA_MOD_HIT_CHANCE = 54
        SPELL_AURA_MOD_SPELL_HIT_CHANCE = 55
        SPELL_AURA_TRANSFORM = 56
        SPELL_AURA_MOD_SPELL_CRIT_CHANCE = 57
        SPELL_AURA_MOD_INCREASE_SWIM_SPEED = 58
        SPELL_AURA_MOD_DAMAGE_DONE_CREATURE = 59
        SPELL_AURA_MOD_PACIFY_SILENCE = 60
        SPELL_AURA_MOD_SCALE = 61
        SPELL_AURA_PERIODIC_HEALTH_FUNNEL = 62
        SPELL_AURA_PERIODIC_MANA_FUNNEL = 63
        SPELL_AURA_PERIODIC_MANA_LEECH = 64
        SPELL_AURA_MOD_CASTING_SPEED = 65
        SPELL_AURA_FEIGN_DEATH = 66
        SPELL_AURA_MOD_DISARM = 67
        SPELL_AURA_MOD_STALKED = 68
        SPELL_AURA_SCHOOL_ABSORB = 69
        SPELL_AURA_EXTRA_ATTACKS = 70
        SPELL_AURA_MOD_SPELL_CRIT_CHANCE_SCHOOL = 71
        SPELL_AURA_MOD_POWER_COST_SCHOOL_PCT = 72
        SPELL_AURA_MOD_POWER_COST_SCHOOL = 73
        SPELL_AURA_REFLECT_SPELLS_SCHOOL = 74
        SPELL_AURA_MOD_LANGUAGE = 75
        SPELL_AURA_FAR_SIGHT = 76
        SPELL_AURA_MECHANIC_IMMUNITY = 77
        SPELL_AURA_MOUNTED = 78
        SPELL_AURA_MOD_DAMAGE_PERCENT_DONE = 79
        SPELL_AURA_MOD_PERCENT_STAT = 80
        SPELL_AURA_SPLIT_DAMAGE_PCT = 81
        SPELL_AURA_WATER_BREATHING = 82
        SPELL_AURA_MOD_BASE_RESISTANCE = 83
        SPELL_AURA_MOD_REGEN = 84
        SPELL_AURA_MOD_POWER_REGEN = 85
        SPELL_AURA_CHANNEL_DEATH_ITEM = 86
        SPELL_AURA_MOD_DAMAGE_PERCENT_TAKEN = 87
        SPELL_AURA_MOD_HEALTH_REGEN_PERCENT = 88
        SPELL_AURA_PERIODIC_DAMAGE_PERCENT = 89
        SPELL_AURA_MOD_RESIST_CHANCE = 90
        SPELL_AURA_MOD_DETECT_RANGE = 91
        SPELL_AURA_PREVENTS_FLEEING = 92
        SPELL_AURA_MOD_UNATTACKABLE = 93
        SPELL_AURA_INTERRUPT_REGEN = 94
        SPELL_AURA_GHOST = 95
        SPELL_AURA_SPELL_MAGNET = 96
        SPELL_AURA_MANA_SHIELD = 97
        SPELL_AURA_MOD_SKILL_TALENT = 98
        SPELL_AURA_MOD_ATTACK_POWER = 99
        SPELL_AURA_AURAS_VISIBLE = 100
        SPELL_AURA_MOD_RESISTANCE_PCT = 101
        SPELL_AURA_MOD_MELEE_ATTACK_POWER_VERSUS = 102
        SPELL_AURA_MOD_TOTAL_THREAT = 103
        SPELL_AURA_WATER_WALK = 104
        SPELL_AURA_FEATHER_FALL = 105
        SPELL_AURA_HOVER = 106
        SPELL_AURA_ADD_FLAT_MODIFIER = 107
        SPELL_AURA_ADD_PCT_MODIFIER = 108
        SPELL_AURA_ADD_TARGET_TRIGGER = 109
        SPELL_AURA_MOD_POWER_REGEN_PERCENT = 110
        SPELL_AURA_ADD_CASTER_HIT_TRIGGER = 111
        SPELL_AURA_OVERRIDE_CLASS_SCRIPTS = 112
        SPELL_AURA_MOD_RANGED_DAMAGE_TAKEN = 113
        SPELL_AURA_MOD_RANGED_DAMAGE_TAKEN_PCT = 114
        SPELL_AURA_MOD_HEALING = 115
        SPELL_AURA_MOD_REGEN_DURING_COMBAT = 116
        SPELL_AURA_MOD_MECHANIC_RESISTANCE = 117
        SPELL_AURA_MOD_HEALING_PCT = 118
        SPELL_AURA_SHARE_PET_TRACKING = 119
        SPELL_AURA_UNTRACKABLE = 120
        SPELL_AURA_EMPATHY = 121
        SPELL_AURA_MOD_OFFHAND_DAMAGE_PCT = 122
        SPELL_AURA_MOD_TARGET_RESISTANCE = 123
        SPELL_AURA_MOD_RANGED_ATTACK_POWER = 124
        SPELL_AURA_MOD_MELEE_DAMAGE_TAKEN = 125
        SPELL_AURA_MOD_MELEE_DAMAGE_TAKEN_PCT = 126
        SPELL_AURA_RANGED_ATTACK_POWER_ATTACKER_BONUS = 127
        SPELL_AURA_MOD_POSSESS_PET = 128
        SPELL_AURA_MOD_SPEED_ALWAYS = 129
        SPELL_AURA_MOD_MOUNTED_SPEED_ALWAYS = 130
        SPELL_AURA_MOD_RANGED_ATTACK_POWER_VERSUS = 131
        SPELL_AURA_MOD_INCREASE_ENERGY_PERCENT = 132
        SPELL_AURA_MOD_INCREASE_HEALTH_PERCENT = 133
        SPELL_AURA_MOD_MANA_REGEN_INTERRUPT = 134
        SPELL_AURA_MOD_HEALING_DONE = 135
        SPELL_AURA_MOD_HEALING_DONE_PERCENT = 136
        SPELL_AURA_MOD_TOTAL_STAT_PERCENTAGE = 137
        SPELL_AURA_MOD_HASTE = 138
        SPELL_AURA_FORCE_REACTION = 139
        SPELL_AURA_MOD_RANGED_HASTE = 140
        SPELL_AURA_MOD_RANGED_AMMO_HASTE = 141
        SPELL_AURA_MOD_BASE_RESISTANCE_PCT = 142
        SPELL_AURA_MOD_RESISTANCE_EXCLUSIVE = 143
        SPELL_AURA_SAFE_FALL = 144
        SPELL_AURA_CHARISMA = 145
        SPELL_AURA_PERSUADED = 146
        SPELL_AURA_ADD_CREATURE_IMMUNITY = 147
        SPELL_AURA_RETAIN_COMBO_POINTS = 148
        SPELL_AURA_RESIST_PUSHBACK = 149                      '    Resist Pushback
        SPELL_AURA_MOD_SHIELD_BLOCKVALUE_PCT = 150
        SPELL_AURA_TRACK_STEALTHED = 151                      '    Track Stealthed
        SPELL_AURA_MOD_DETECTED_RANGE = 152                    '    Mod Detected Range
        SPELL_AURA_SPLIT_DAMAGE_FLAT = 153                     '    Split Damage Flat
        SPELL_AURA_MOD_STEALTH_LEVEL = 154                     '    Stealth Level Modifier
        SPELL_AURA_MOD_WATER_BREATHING = 155                   '    Mod Water Breathing
        SPELL_AURA_MOD_REPUTATION_GAIN = 156                   '    Mod Reputation Gain
        SPELL_AURA_PET_DAMAGE_MULTI = 157                      '    Mod Pet Damage
        SPELL_AURA_MOD_SHIELD_BLOCKVALUE = 158
        SPELL_AURA_NO_PVP_CREDIT = 159
        SPELL_AURA_MOD_AOE_AVOIDANCE = 160
        SPELL_AURA_MOD_HEALTH_REGEN_IN_COMBAT = 161
        SPELL_AURA_POWER_BURN_MANA = 162
        SPELL_AURA_MOD_CRIT_DAMAGE_BONUS_MELEE = 163
        SPELL_AURA_164 = 164
        SPELL_AURA_MELEE_ATTACK_POWER_ATTACKER_BONUS = 165
        SPELL_AURA_MOD_ATTACK_POWER_PCT = 166
        SPELL_AURA_MOD_RANGED_ATTACK_POWER_PCT = 167
        SPELL_AURA_MOD_DAMAGE_DONE_VERSUS = 168
        SPELL_AURA_MOD_CRIT_PERCENT_VERSUS = 169
        SPELL_AURA_DETECT_AMORE = 170
        SPELL_AURA_MOD_SPEED_NOT_STACK = 171
        SPELL_AURA_MOD_MOUNTED_SPEED_NOT_STACK = 172
        SPELL_AURA_ALLOW_CHAMPION_SPELLS = 173
        SPELL_AURA_MOD_SPELL_DAMAGE_OF_STAT_PERCENT = 174      ' by defeult intelect dependent from SPELL_AURA_MOD_SPELL_HEALING_OF_STAT_PERCENT
        SPELL_AURA_MOD_SPELL_HEALING_OF_STAT_PERCENT = 175
        SPELL_AURA_SPIRIT_OF_REDEMPTION = 176
        SPELL_AURA_AOE_CHARM = 177
        SPELL_AURA_MOD_DEBUFF_RESISTANCE = 178
        SPELL_AURA_MOD_ATTACKER_SPELL_CRIT_CHANCE = 179
        SPELL_AURA_MOD_FLAT_SPELL_DAMAGE_VERSUS = 180
        SPELL_AURA_MOD_FLAT_SPELL_CRIT_DAMAGE_VERSUS = 181     ' unused - possible flat spell crit damage versus
        SPELL_AURA_MOD_RESISTANCE_OF_STAT_PERCENT = 182
        SPELL_AURA_MOD_CRITICAL_THREAT = 183
        SPELL_AURA_MOD_ATTACKER_MELEE_HIT_CHANCE = 184
        SPELL_AURA_MOD_ATTACKER_RANGED_HIT_CHANCE = 185
        SPELL_AURA_MOD_ATTACKER_SPELL_HIT_CHANCE = 186
        SPELL_AURA_MOD_ATTACKER_MELEE_CRIT_CHANCE = 187
        SPELL_AURA_MOD_ATTACKER_RANGED_CRIT_CHANCE = 188
        SPELL_AURA_MOD_RATING = 189
        SPELL_AURA_MOD_FACTION_REPUTATION_GAIN = 190
        SPELL_AURA_USE_NORMAL_MOVEMENT_SPEED = 191
        SPELL_AURA_HASTE_MELEE = 192
        SPELL_AURA_MELEE_SLOW = 193
        SPELL_AURA_MOD_DEPRICATED_1 = 194                     ' not used now old SPELL_AURA_MOD_SPELL_DAMAGE_OF_INTELLECT
        SPELL_AURA_MOD_DEPRICATED_2 = 195                     ' not used now old SPELL_AURA_MOD_SPELL_HEALING_OF_INTELLECT
        SPELL_AURA_MOD_COOLDOWN = 196                          ' only 24818 Noxious Breath
        SPELL_AURA_MOD_ATTACKER_SPELL_AND_WEAPON_CRIT_CHANCE = 197
        SPELL_AURA_MOD_ALL_WEAPON_SKILLS = 198
        SPELL_AURA_MOD_INCREASES_SPELL_PCT_TO_HIT = 199
        SPELL_AURA_MOD_XP_PCT = 200
        SPELL_AURA_FLY = 201
        SPELL_AURA_IGNORE_COMBAT_RESULT = 202
        SPELL_AURA_MOD_ATTACKER_MELEE_CRIT_DAMAGE = 203
        SPELL_AURA_MOD_ATTACKER_RANGED_CRIT_DAMAGE = 204
        SPELL_AURA_205 = 205                                   ' unused
        SPELL_AURA_MOD_SPEED_MOUNTED = 206                     ' ? used in strange spells
        SPELL_AURA_MOD_INCREASE_FLIGHT_SPEED = 207
        SPELL_AURA_MOD_SPEED_FLIGHT = 208
        SPELL_AURA_MOD_FLIGHT_SPEED_ALWAYS = 209
        SPELL_AURA_210 = 210                                   ' unused
        SPELL_AURA_MOD_FLIGHT_SPEED_NOT_STACK = 211
        SPELL_AURA_MOD_RANGED_ATTACK_POWER_OF_STAT_PERCENT = 212
        SPELL_AURA_MOD_RAGE_FROM_DAMAGE_DEALT = 213
        SPELL_AURA_214 = 214
        SPELL_AURA_ARENA_PREPARATION = 215
        SPELL_AURA_HASTE_SPELLS = 216
        SPELL_AURA_217 = 217
        SPELL_AURA_HASTE_RANGED = 218
        SPELL_AURA_MOD_MANA_REGEN_FROM_STAT = 219
        SPELL_AURA_MOD_RATING_FROM_STAT = 220
        SPELL_AURA_221 = 221
        SPELL_AURA_222 = 222
        SPELL_AURA_223 = 223
        SPELL_AURA_224 = 224
        SPELL_AURA_PRAYER_OF_MENDING = 225
        SPELL_AURA_PERIODIC_DUMMY = 226
        SPELL_AURA_227 = 227
        SPELL_AURA_DETECT_STEALTH = 228
        SPELL_AURA_MOD_AOE_DAMAGE_AVOIDANCE = 229
        SPELL_AURA_230 = 230
        SPELL_AURA_231 = 231
        SPELL_AURA_MECHANIC_DURATION_MOD = 232
        SPELL_AURA_233 = 233
        SPELL_AURA_MECHANIC_DURATION_MOD_NOT_STACK = 234
        SPELL_AURA_MOD_DISPEL_RESIST = 235
        SPELL_AURA_236 = 236
        SPELL_AURA_MOD_SPELL_DAMAGE_OF_ATTACK_POWER = 237
        SPELL_AURA_MOD_SPELL_HEALING_OF_ATTACK_POWER = 238
        SPELL_AURA_MOD_SCALE_2 = 239
        SPELL_AURA_MOD_EXPERTISE = 240
        SPELL_AURA_241 = 241
        SPELL_AURA_MOD_SPELL_DAMAGE_FROM_HEALING = 242
        SPELL_AURA_243 = 243
        SPELL_AURA_244 = 244
        SPELL_AURA_MOD_DURATION_OF_MAGIC_EFFECTS = 245
        SPELL_AURA_246 = 246
        SPELL_AURA_247 = 247
        SPELL_AURA_MOD_COMBAT_RESULT_CHANCE = 248
        SPELL_AURA_249 = 249
        SPELL_AURA_MOD_INCREASE_HEALTH_2 = 250
        SPELL_AURA_MOD_ENEMY_DODGE = 251
        SPELL_AURA_252 = 252
        SPELL_AURA_253 = 253
        SPELL_AURA_254 = 254
        SPELL_AURA_255 = 255
        SPELL_AURA_256 = 256
        SPELL_AURA_257 = 257
        SPELL_AURA_258 = 258
        SPELL_AURA_259 = 259
        SPELL_AURA_260 = 260
        SPELL_AURA_261 = 261
    End Enum

    Delegate Function SpellEffectHandler(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef value As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
    Public Class SpellEffectParameter
        Public Caster As BaseObject

        Public Target As BaseObject
        Public SourceX As Single = 0
        Public SourceY As Single = 0
        Public SourceZ As Single = 0
        Public DestinationX As Single = 0
        Public DestinationY As Single = 0
        Public DestinationZ As Single = 0
    End Class
    Public Const SPELL_EFFECT_COUNT As Integer = 135
    Public SPELL_EFFECTs(SPELL_EFFECT_COUNT) As SpellEffectHandler

    Public Function SPELL_EFFECT_NOTHING(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef value As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If Not Target.unitTarget Is Nothing Then Infected.Add(Target.unitTarget.GUID)
        If Not Target.goTarget Is Nothing Then Infected.Add(Target.goTarget.GUID)
        If Not Target.itemTarget Is Nothing Then Infected.Add(Target.itemTarget.GUID)
        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_BIND(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef value As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Infected.Add(Target.unitTarget.GUID)
        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_DUMMY(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef value As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If Not Target.unitTarget Is Nothing Then Infected.Add(Target.unitTarget.GUID)
        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function SPELL_EFFECT_INSTAKILL(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_AREA_EFFECT_ENEMY
                'TODO: Area effects
            Case Else
                Target.unitTarget.Die(Caster)
                Infected.Add(Target.unitTarget.GUID)
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_SCHOOL_DAMAGE(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Dim Damage As Integer = SpellInfo.GetValue(Caster.Level)
        Dim IsCrit As Boolean = False

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELECTED_ENEMY
                If TypeOf Caster Is CharacterObject Then
                    If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                        Damage = Fix(1.5F * Damage)
                        IsCrit = True
                    End If
                End If

                Target.unitTarget.DealDamage(Damage, Caster)
                Infected.Add(Target.unitTarget.GUID)
                SendNonMeleeDamageLog(Caster, Target.unitTarget, SpellID, Damage, 0, 0, IsCrit)
            Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                Dim tmpDamage As Integer = Damage
                Dim Targets As List(Of BaseUnit) = GetEnemyAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    IsCrit = False
                    If TypeOf Caster Is CharacterObject Then
                        If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                            tmpDamage = Fix(1.5F * tmpDamage)
                            IsCrit = True
                        End If
                    End If

                    unit.DealDamage(tmpDamage, Caster)
                    Infected.Add(unit.GUID)
                    SendNonMeleeDamageLog(Caster, unit, SpellID, tmpDamage, 0, 0, IsCrit)
                Next
            Case SpellImplicitTargets.TARGET_CHAIN

            Case SpellImplicitTargets.TARGET_INFRONT

            Case SpellImplicitTargets.TARGET_AREA_EFFECT_ENEMY, SpellImplicitTargets.TARGET_AREA_EFFECT_ENEMY_CHANNEL, SpellImplicitTargets.TARGET_AREA_EFFECT_ENEMY_INSTANT
                Dim tmpDamage As Integer = Damage
                Dim Targets As List(Of BaseUnit) = GetEnemyAtPoint(Caster, Target.dstX, Target.dstY, Target.dstZ, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    IsCrit = False
                    If TypeOf Caster Is CharacterObject Then
                        If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                            tmpDamage = Fix(1.5F * tmpDamage)
                            IsCrit = True
                        End If
                    End If

                    unit.DealDamage(tmpDamage, Caster)
                    Infected.Add(unit.GUID)
                    SendNonMeleeDamageLog(Caster, unit, SpellID, tmpDamage, 0, 0, IsCrit)
                Next
            Case Else
                Target.unitTarget.DealDamage(Damage, Caster)
                Infected.Add(Target.unitTarget.GUID)
                SendNonMeleeDamageLog(Caster, Target.unitTarget, SpellID, Damage, 0, 0, IsCrit)
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_ENVIRONMENTAL_DAMAGE(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Dim Damage As Integer = SpellInfo.GetValue(Caster.Level)

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELECTED_ENEMY
                Target.unitTarget.DealDamage(Damage, Caster)
                Infected.Add(Target.unitTarget.GUID)
                If TypeOf Target.unitTarget Is CharacterObject Then CType(Target.unitTarget, CharacterObject).LogEnvironmentalDamage(SPELLs(SpellID).School, Damage)

            Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                Dim Targets As List(Of BaseUnit) = GetEnemyAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    unit.DealDamage(Damage, Caster)
                    Infected.Add(unit.GUID)
                    If TypeOf unit Is CharacterObject Then CType(unit, CharacterObject).LogEnvironmentalDamage(SPELLs(SpellID).School, Damage)
                Next

            Case Else
                Target.unitTarget.DealDamage(Damage, Caster)
                Infected.Add(Target.unitTarget.GUID)
                If TypeOf Target.unitTarget Is CharacterObject Then CType(Target.unitTarget, CharacterObject).LogEnvironmentalDamage(SPELLs(SpellID).School, Damage)
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_TRIGGER_SPELL(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        'NOTE: Trigger spell shouldn't add a cast error?
        If SPELLs.ContainsKey(SpellInfo.TriggerSpell) = False Then Return SpellFailedReason.CAST_NO_ERROR
        If Target.unitTarget Is Nothing Then Return SpellFailedReason.CAST_NO_ERROR

        Select Case SpellInfo.TriggerSpell
            Case 18461
                Target.unitTarget.RemoveAurasOfType(AuraEffects_Names.SPELL_AURA_MOD_ROOT)
                Target.unitTarget.RemoveAurasOfType(AuraEffects_Names.SPELL_AURA_MOD_DECREASE_SPEED)
                Target.unitTarget.RemoveAurasOfType(AuraEffects_Names.SPELL_AURA_MOD_STALKED)

                'TODO: Cast highest rank of stealth
            Case 35729
                For i As Byte = MAX_POSITIVE_AURA_EFFECTs To MAX_AURA_EFFECTs_VISIBLE - 1
                    If Not Target.unitTarget.ActiveSpells(i) Is Nothing Then
                        If (SPELLs(Target.unitTarget.ActiveSpells(i).SpellID).School And 1) = 0 Then 'No physical spells
                            If (SPELLs(Target.unitTarget.ActiveSpells(i).SpellID).Attributes And &H10000) Then
                                Target.unitTarget.RemoveAura(i, Target.unitTarget.ActiveSpells(i).SpellCaster)
                            End If
                        End If
                    End If
                Next
        End Select

        If SPELLs(SpellInfo.TriggerSpell).EquippedItemClass >= 0 And (TypeOf Caster Is CharacterObject) Then
            If (SPELLs(SpellInfo.TriggerSpell).AttributesEx3 And SpellAttributesEx3.SPELL_ATTR_EX3_MAIN_HAND) Then
                If CType(Caster, CharacterObject).Items.ContainsKey(EQUIPMENT_SLOT_MAINHAND) = False Then Return SpellFailedReason.CAST_NO_ERROR
                If CType(Caster, CharacterObject).Items(EQUIPMENT_SLOT_MAINHAND).IsBroken Then Return SpellFailedReason.CAST_NO_ERROR
            End If
            If (SPELLs(SpellInfo.TriggerSpell).AttributesEx3 And SpellAttributesEx3.SPELL_ATTR_EX3_REQ_OFFHAND) Then
                If CType(Caster, CharacterObject).Items.ContainsKey(EQUIPMENT_SLOT_OFFHAND) = False Then Return SpellFailedReason.CAST_NO_ERROR
                If CType(Caster, CharacterObject).Items(EQUIPMENT_SLOT_OFFHAND).IsBroken Then Return SpellFailedReason.CAST_NO_ERROR
            End If
        End If

        SPELLs(SpellInfo.TriggerSpell).Cast(Caster, Target, 0)

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_TELEPORT_UNITS(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If GuidIsPlayer(Target.unitTarget.GUID) Then
            With CType(Target.unitTarget, CharacterObject)
                Select Case SpellID
                    Case 8690 'Hearthstone
                        .Teleport(.bindpoint_positionX, .bindpoint_positionY, .bindpoint_positionZ, .orientation, .bindpoint_map_id)
                    Case Else
                        If TeleportCoords.ContainsKey(SpellID) Then
                            .Teleport(TeleportCoords(SpellID).PosX, TeleportCoords(SpellID).PosY, TeleportCoords(SpellID).PosZ, .orientation, TeleportCoords(SpellID).MapID)
                        Else
                            Log.WriteLine(LogType.WARNING, "WARNING: Spell {0} did not have any teleport coordinates.", SpellID)
                        End If
                End Select
            End With
        Else
            Log.WriteLine(LogType.DEBUG, "DEBUG: Casting SPELL_EFFECT_TELEPORT_UNITS on unknown object.")
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_MANA_DRAIN(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If SpellInfo.MiscValue <> 0 Then
            Log.WriteLine(LogType.DEBUG, "DEBUG: Unsupported powerType to drain.")
            Exit Function
        End If

        Dim Damage As Integer = SpellInfo.GetValue(Caster.Level)

        If TypeOf Caster Is CharacterObject Then
            Damage += SpellInfo.valuePerLevel * CType(Caster, CharacterObject).Level
        End If

        '! Only 1 target everytime, so no need to check
        'Select Case SpellInfo.implicitTargetA
        '    Case SpellImplicitTargets.TARGET_SELF, SpellImplicitTargets.TARGET_NOTHING
        '    Case SpellImplicitTargets.TARGET_PET
        '    Case SpellImplicitTargets.TARGET_SELECTED_ENEMY
        'End Select

        'DONE: Get mana from victim
        Target.unitTarget.Mana.Current -= Damage
        'DONE: Give mana to caster
        Caster.Mana.Current += Damage

        'DONE: Send victim mana update, for near
        If GuidIsCreature(Target.unitTarget.GUID) Then
            Dim myTmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
            Dim myPacket As New UpdatePacketClass
            myTmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1 + SpellInfo.MiscValue, CType(CType(Target.unitTarget, CreatureObject).Mana.Current, Integer))
            myTmpUpdate.AddToPacket(CType(myPacket, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target.unitTarget, CreatureObject), 0)
            Target.unitTarget.SendToNearPlayers(CType(myPacket, UpdatePacketClass))
            myPacket.Dispose()
            myTmpUpdate.Dispose()
        ElseIf GuidIsPlayer(Target.unitTarget.GUID) Then
            CType(Caster, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1 + SpellInfo.MiscValue, CType(CType(Target.unitTarget, CharacterObject).Mana.Current, Integer))
        End If

        'TODO: SpellFailedReason.CAST_FAIL_ALREADY_FULL_MANA
        'DONE: Send caster mana update, for near
        If TypeOf Caster Is CreatureObject Then
            Dim TmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
            Dim Packet As New UpdatePacketClass
            TmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1 + SpellInfo.MiscValue, CType(CType(Caster, CreatureObject).Mana.Current, Integer))
            TmpUpdate.AddToPacket(CType(Packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Caster, CreatureObject), 0)
            Target.unitTarget.SendToNearPlayers(CType(Packet, UpdatePacketClass))
            Packet.Dispose()
            TmpUpdate.Dispose()
        ElseIf TypeOf Caster Is CharacterObject Then
            CType(Caster, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1 + SpellInfo.MiscValue, CType(CType(Caster, CharacterObject).Mana.Current, Integer))
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_HEAL(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Dim Damage As Integer = SpellInfo.GetValue(Caster.Level)
        Dim IsCrit As Boolean = False

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELF, SpellImplicitTargets.TARGET_CASTER, SpellImplicitTargets.TARGET_SELECTED_FRIEND, SpellImplicitTargets.TARGET_DUEL_VS_PLAYER, SpellImplicitTargets.TARGET_MASTER
                If TypeOf Caster Is CharacterObject Then
                    If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                        Damage = Fix(1.5F * Damage)
                        IsCrit = True
                    End If
                End If

                Target.unitTarget.Heal(Damage, Caster)
                Infected.Add(Caster.GUID)
                SendHealSpellLog(Caster, Target.unitTarget, SpellID, Damage, IsCrit)

            Case SpellImplicitTargets.TARGET_PET, SpellImplicitTargets.TARGET_SELECTED_ENEMY
                If TypeOf Caster Is CharacterObject Then
                    If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                        Damage = Fix(1.5F * Damage)
                        IsCrit = True
                    End If
                End If

                Target.unitTarget.Heal(Damage, Caster)
                Infected.Add(Target.unitTarget.GUID)
                SendHealSpellLog(Caster, Target.unitTarget, SpellID, Damage, IsCrit)

            Case SpellImplicitTargets.TARGET_AROUND_CASTER_PARTY
                Dim Targets As List(Of BaseUnit) = GetPartyMembersAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    IsCrit = False
                    Dim tmpDmg As Integer = Damage
                    If TypeOf Caster Is CharacterObject Then
                        If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                            tmpDmg = Fix(1.5F * tmpDmg)
                            IsCrit = True
                        End If
                    End If

                    unit.Heal(tmpDmg, Caster)
                    Infected.Add(unit.GUID)
                    SendHealSpellLog(Caster, unit, SpellID, tmpDmg, IsCrit)
                Next

            Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                Dim Targets As List(Of BaseUnit) = GetEnemyAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    IsCrit = False
                    Dim tmpDmg As Integer = Damage
                    If TypeOf Caster Is CharacterObject Then
                        If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                            tmpDmg = Fix(1.5F * tmpDmg)
                            IsCrit = True
                        End If
                    End If

                    unit.Heal(tmpDmg, Caster)
                    Infected.Add(unit.GUID)
                    SendHealSpellLog(Caster, unit, SpellID, tmpDmg, IsCrit)
                Next

            Case SpellImplicitTargets.TARGET_CHAIN
                Log.WriteLine(LogType.DEBUG, "Amplitude: {0}", SpellInfo.Amplitude)
                Log.WriteLine(LogType.DEBUG, "MiscValue: {0}", SpellInfo.MiscValue)
                Log.WriteLine(LogType.DEBUG, "Chain Max Targets: {0}", SpellInfo.ChainTarget)

                If TypeOf Caster Is CharacterObject Then
                    If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                        Damage = Fix(1.5F * Damage)
                        IsCrit = True
                    End If
                End If

                Dim UsedTargets As New List(Of BaseUnit)
                Dim TargetUnit As BaseUnit = Nothing
                Target.unitTarget.Heal(Damage, Caster)
                Infected.Add(Target.unitTarget.GUID)
                SendHealSpellLog(Caster, Target.unitTarget, SpellID, Damage, IsCrit)
                UsedTargets.Add(Target.unitTarget)
                TargetUnit = Target.unitTarget

                If SpellInfo.ChainTarget > 1 Then
                    For i As Byte = 2 To SpellInfo.ChainTarget
                        Damage *= 0.5
                        Dim Targets As List(Of BaseUnit) = GetPartyMembersAroundMe(TargetUnit, 10)
                        TargetUnit = Nothing
                        Dim LowHealth As Single = 1.01
                        Dim TmpLife As Single = 0
                        For Each tmpUnit As BaseUnit In Targets
                            If UsedTargets.Contains(tmpUnit) = False Then
                                TmpLife = (tmpUnit.Life.Current / tmpUnit.Life.Maximum)
                                If TmpLife < LowHealth Then
                                    LowHealth = TmpLife
                                    TargetUnit = tmpUnit
                                End If
                            End If
                        Next
                        If Not TargetUnit Is Nothing Then
                            TargetUnit.Heal(Damage, Caster)
                            Infected.Add(TargetUnit.GUID)
                            SendHealSpellLog(Caster, TargetUnit, SpellID, Damage, IsCrit)
                            UsedTargets.Add(TargetUnit)
                        Else
                            Exit For
                        End If
                    Next
                End If
            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_HEAL_MAX_HEALTH(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Dim Damage As Integer = Caster.Life.Maximum
        Dim IsCrit As Boolean = False

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELF, SpellImplicitTargets.TARGET_CASTER, SpellImplicitTargets.TARGET_SELECTED_FRIEND, SpellImplicitTargets.TARGET_DUEL_VS_PLAYER, SpellImplicitTargets.TARGET_MASTER
                If TypeOf Caster Is CharacterObject Then
                    If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                        Damage = Fix(1.5F * Damage)
                        IsCrit = True
                    End If
                End If

                Caster.Heal(Damage, Caster)
                Infected.Add(Caster.GUID)
                SendHealSpellLog(Caster, Target.unitTarget, SpellID, Damage, IsCrit)

            Case SpellImplicitTargets.TARGET_PET, SpellImplicitTargets.TARGET_SELECTED_ENEMY
                If TypeOf Caster Is CharacterObject Then
                    If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                        Damage = Fix(1.5F * Damage)
                        IsCrit = True
                    End If
                End If

                Target.unitTarget.Heal(Damage, Caster)
                Infected.Add(Target.unitTarget.GUID)
                SendHealSpellLog(Caster, Target.unitTarget, SpellID, Damage, IsCrit)

            Case SpellImplicitTargets.TARGET_AROUND_CASTER_PARTY
                Dim Targets As List(Of BaseUnit) = GetPartyMembersAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    IsCrit = False
                    Dim tmpDmg As Integer = Damage
                    If TypeOf Caster Is CharacterObject Then
                        If (CType(Caster, CharacterObject).GetCriticalWithSpells + 5) > Rnd.Next(0, 100) Then
                            tmpDmg = Fix(1.5F * tmpDmg)
                            IsCrit = True
                        End If
                    End If

                    unit.Heal(tmpDmg, Caster)
                    Infected.Add(unit.GUID)
                    SendHealSpellLog(Caster, unit, SpellID, tmpDmg, IsCrit)
                Next

            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_ENERGIZE(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Dim Damage As Integer = SpellInfo.GetValue(Caster.Level)

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELF, SpellImplicitTargets.TARGET_CASTER
                Caster.Energize(Damage, SpellInfo.MiscValue, Caster)
                Infected.Add(Caster.GUID)

                SendEnergizeSpellLog(Caster, Target.unitTarget, SpellID, Damage, SpellInfo.MiscValue)

            Case SpellImplicitTargets.TARGET_PET, SpellImplicitTargets.TARGET_SELECTED_FRIEND
                Target.unitTarget.Energize(Damage, SpellInfo.MiscValue, Caster)
                Infected.Add(Target.unitTarget.GUID)
                SendEnergizeSpellLog(Caster, Target.unitTarget, SpellID, Damage, SpellInfo.MiscValue)

            Case SpellImplicitTargets.TARGET_AROUND_CASTER_PARTY
                Dim Targets As List(Of BaseUnit) = GetPartyMembersAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    unit.Energize(Damage, SpellInfo.MiscValue, Caster)
                    Infected.Add(unit.GUID)
                    SendEnergizeSpellLog(Caster, unit, SpellID, Damage, SpellInfo.MiscValue)
                Next

            Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                Dim Targets As List(Of BaseUnit) = GetEnemyAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    unit.Energize(Damage, SpellInfo.MiscValue, Caster)
                    Infected.Add(unit.GUID)
                    SendEnergizeSpellLog(Caster, unit, SpellID, Damage, SpellInfo.MiscValue)
                Next

            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function SPELL_EFFECT_OPEN_LOCK(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If Not TypeOf Caster Is CharacterObject Then Return SpellFailedReason.CAST_FAIL_ERROR

        Dim LootType As LootType = WS_Loot.LootType.LOOTTYPE_CORPSE

        Dim targetGUID As ULong, lockID As Integer
        If Not Target.goTarget Is Nothing Then 'GO Target
            targetGUID = Target.goTarget.GUID
            lockID = CType(Target.goTarget, GameObjectObject).LockID
            Infected.Add(Target.goTarget.GUID)
        ElseIf Not Target.itemTarget Is Nothing Then 'Item Target
            targetGUID = Target.itemTarget.GUID
            lockID = Target.itemTarget.ItemInfo.LockID
            Infected.Add(Target.itemTarget.GUID)
        Else
            Return SpellFailedReason.CAST_FAIL_BAD_TARGETS
        End If

        'TODO: Check if it's a battlegroundflag

        If lockID = 0 Then
            'TODO: Send loot for items
            If GuidIsGameObject(targetGUID) AndAlso WORLD_GAMEOBJECTs.ContainsKey(targetGUID) Then
                CType(WORLD_GAMEOBJECTs(targetGUID), GameObjectObject).LootObject(CType(Caster, CharacterObject), LootType)
            End If

            CType(Caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_FINISHED
            Return SpellFailedReason.CAST_NO_ERROR
        End If

        If Locks.ContainsKey(lockID) = False Then
            Return SpellFailedReason.CAST_FAIL_ERROR
        End If
        Dim i As Byte
        For i = 0 To 4
            If CType(Locks(lockID), TLock).KeyType(i) = LockKeyType.LOCK_KEY_ITEM AndAlso CType(Locks(lockID), TLock).KeyType(i) > 0 AndAlso CType(Locks(lockID), TLock).KeyType(i) = Item.ItemEntry Then
                'TODO: Send loot for items
                If GuidIsGameObject(targetGUID) AndAlso WORLD_GAMEOBJECTs.ContainsKey(targetGUID) Then
                    CType(WORLD_GAMEOBJECTs(targetGUID), GameObjectObject).LootObject(CType(Caster, CharacterObject), LootType)
                End If

                CType(Caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_FINISHED
                Return SpellFailedReason.CAST_NO_ERROR
            End If
        Next

        Dim SkillID As Integer = 0
        If (Not CType(SPELLs(SpellID), SpellInfo).SpellEffects(1) Is Nothing) AndAlso CType(SPELLs(SpellID), SpellInfo).SpellEffects(1).ID = SpellEffects_Names.SPELL_EFFECT_SKILL Then
            SkillID = CType(SPELLs(SpellID), SpellInfo).SpellEffects(1).MiscValue
        ElseIf (Not CType(SPELLs(SpellID), SpellInfo).SpellEffects(0) Is Nothing) AndAlso CType(SPELLs(SpellID), SpellInfo).SpellEffects(0).MiscValue = LockType.LOCKTYPE_PICKLOCK Then
            SkillID = SKILL_IDs.SKILL_LOCKPICKING
        End If

        Dim ReqSkillValue As Short = CType(Locks(lockID), TLock).RequiredMiningSkill
        If CType(Locks(lockID), TLock).RequiredLockingSkill > 0 Then
            If SkillID <> SKILL_IDs.SKILL_LOCKPICKING Then 'Cheat attempt?
                Return SpellFailedReason.CAST_FAIL_FIZZLE
            End If
            ReqSkillValue = CType(Locks(lockID), TLock).RequiredLockingSkill
        ElseIf SkillID = SKILL_IDs.SKILL_LOCKPICKING Then 'Apply picklock skill to wrong target
            Return SpellFailedReason.CAST_FAIL_BAD_TARGETS
        End If

        If SkillID Then
            LootType = LootType.LOOTTYPE_SKINNNING
            If CType(Caster, CharacterObject).Skills.ContainsKey(SkillID) = False OrElse CType(CType(Caster, CharacterObject).Skills(SkillID), TSkill).Current < ReqSkillValue Then
                Return SpellFailedReason.CAST_FAIL_LOW_CASTLEVEL
            End If

            'TODO: Update skill
        End If

        'TODO: Send loot for items
        If GuidIsGameObject(targetGUID) AndAlso WORLD_GAMEOBJECTs.ContainsKey(targetGUID) Then
            CType(WORLD_GAMEOBJECTs(targetGUID), GameObjectObject).LootObject(CType(Caster, CharacterObject), LootType)
        End If

        CType(Caster, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_FINISHED
        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function SPELL_EFFECT_LEARN_SPELL(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELF
                If SpellInfo.TriggerSpell <> 0 Then
                    CType(Caster, CharacterObject).LearnSpell(SpellInfo.TriggerSpell)
                    Infected.Add(Target.unitTarget.GUID)
                End If
            Case SpellImplicitTargets.TARGET_PET
                Return SpellFailedReason.CAST_FAIL_TOO_MANY_SKILLS
            Case Else
                If TypeOf Target.unitTarget Is CharacterObject Then
                    If SpellInfo.TriggerSpell <> 0 Then
                        CType(Target.unitTarget, CharacterObject).LearnSpell(SpellInfo.TriggerSpell)
                        Infected.Add(Target.unitTarget.GUID)
                    End If
                End If
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_SKILL_STEP(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If TypeOf Target.unitTarget Is CharacterObject Then
            If SpellInfo.MiscValue <> 0 Then
                CType(Target.unitTarget, CharacterObject).LearnSkill(SpellInfo.MiscValue, , (SpellInfo.valueBase + 1) * 75)
                Infected.Add(Target.unitTarget.GUID)
            End If
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_EVADE(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If TypeOf Target.unitTarget Is CreatureObject Then
            If Not CType(Target.unitTarget, CreatureObject).aiScript Is Nothing Then
                CType(Target.unitTarget, CreatureObject).aiScript.State = WS_Creatures_AI.TBaseAI.AIState.AI_EVADE
                Infected.Add(Target.unitTarget.GUID)
            End If
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_DODGE(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If TypeOf Target.unitTarget Is CharacterObject Then
            CType(Target.unitTarget, CharacterObject).combatDodge += SpellInfo.GetValue(Caster.Level)
            Infected.Add(Target.unitTarget.GUID)
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_PARRY(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If TypeOf Target.unitTarget Is CharacterObject Then
            CType(Target.unitTarget, CharacterObject).combatParry += SpellInfo.GetValue(Caster.Level)
            Infected.Add(Target.unitTarget.GUID)
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_BLOCK(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If TypeOf Target.unitTarget Is CharacterObject Then
            CType(Target.unitTarget, CharacterObject).combatBlock += SpellInfo.GetValue(Caster.Level)
            Infected.Add(Target.unitTarget.GUID)
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_DUAL_WIELD(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If TypeOf Target.unitTarget Is CharacterObject Then
            CType(Target.unitTarget, CharacterObject).spellCanDualWeild = True
            Infected.Add(Target.unitTarget.GUID)
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function SPELL_EFFECT_WEAPON_DAMAGE_NOSCHOOL(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If TypeOf Caster Is CharacterObject Then
            If CType(Caster, CharacterObject).attackState.combatNextAttackSpell Then Return SpellFailedReason.CAST_FAIL_SPELL_IN_PROGRESS

            CType(Caster, CharacterObject).attackState.combatNextAttackSpell = True
            CType(Caster, CharacterObject).attackState.combatNextAttack.WaitOne()
        Else
            'TODO: Creatures still cant cast this spell
            Return SpellFailedReason.CAST_FAIL_NOT_READY
        End If

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELECTED_ENEMY
                Dim damageInfo As DamageInfo = CalculateDamage(Caster, Target.unitTarget, False)
                CType(Caster, CharacterObject).attackState.DoMeleeDamageBySpell(Caster, CType(Target.unitTarget, BaseUnit), SpellInfo.GetValue(Caster.Level), SpellID)
                Infected.Add(Target.unitTarget.GUID)

            Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                Dim Targets As List(Of BaseUnit) = GetEnemyAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    CType(Caster, CharacterObject).attackState.DoMeleeDamageBySpell(Caster, CType(unit, BaseUnit), SpellInfo.GetValue(Caster.Level), SpellID)
                    Infected.Add(unit.GUID)
                Next

            Case SpellImplicitTargets.TARGET_UNK54
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function SPELL_EFFECT_HONOR(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        If TypeOf Target.unitTarget Is CharacterObject Then
            CType(Target.unitTarget, CharacterObject).HonorCurrency += SpellInfo.GetValue(Caster.Level)
            CType(Target.unitTarget, CharacterObject).HonorSave()
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Private Const SLOT_NOT_FOUND As Integer = -1
    Private Const SLOT_CREATE_NEW As Integer = -2
    Private Const SLOT_NO_SPACE As Integer = Integer.MaxValue
    Public Function ApplyAura(ByRef auraTarget As BaseUnit, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer) As SpellFailedReason
        Try
            Dim spellCasted As Integer = SLOT_NOT_FOUND
            Do
                'DONE: If active add to visible
                'TODO: If positive effect add to upper part spells
                Dim AuraStart As Integer = MAX_AURA_EFFECTs_VISIBLE - 1
                Dim AuraEnd As Integer = 0

                'DONE: Passives are not displayed
                If CType(SPELLs(SpellID), SpellInfo).IsPassive Then
                    AuraStart = MAX_AURA_EFFECTs - 1
                    AuraEnd = MAX_AURA_EFFECTs_VISIBLE
                End If

                'DONE: Get spell duration
                Dim Duration As Integer = SPELLs(SpellID).GetDuration

                'HACK: Set duration for Resurrection Sickness spell
                If SpellID = 15007 Then
                    Select Case auraTarget.Level
                        Case Is < 11
                            Duration = 0
                        Case Is > 19
                            Duration = 10 * 60 * 1000
                        Case Else
                            Duration = (auraTarget.Level - 10) * 60 * 1000
                    End Select
                End If

                'DONE: Find spell aura slot
                For i As Integer = AuraStart To AuraEnd Step -1
                    If (Not auraTarget.ActiveSpells(i) Is Nothing) AndAlso auraTarget.ActiveSpells(i).SpellID = SpellID Then
                        spellCasted = i
                        If ((Not auraTarget.ActiveSpells(i).Aura1_Info Is Nothing) AndAlso auraTarget.ActiveSpells(i).Aura1_Info Is SpellInfo) OrElse ((Not auraTarget.ActiveSpells(i).Aura2_Info Is Nothing) AndAlso auraTarget.ActiveSpells(i).Aura2_Info Is SpellInfo) OrElse ((Not auraTarget.ActiveSpells(i).Aura3_Info Is Nothing) AndAlso auraTarget.ActiveSpells(i).Aura3_Info Is SpellInfo) Then
                            If (Not auraTarget.ActiveSpells(i).Aura1_Info Is Nothing) AndAlso auraTarget.ActiveSpells(i).Aura1_Info Is SpellInfo Then
                                'DONE: Update the duration
                                auraTarget.ActiveSpells(i).SpellDuration = Duration
                                If TypeOf auraTarget Is CharacterObject Then
                                    Dim SMSG_UPDATE_AURA_DURATION As New PacketClass(OPCODES.SMSG_UPDATE_AURA_DURATION)
                                    SMSG_UPDATE_AURA_DURATION.AddInt8(i)
                                    SMSG_UPDATE_AURA_DURATION.AddInt32(Duration)
                                    CType(auraTarget, CharacterObject).Client.Send(SMSG_UPDATE_AURA_DURATION)
                                    SMSG_UPDATE_AURA_DURATION.Dispose()
                                End If
                                'DONE: Update the stack if possible
                                If CType(SPELLs(SpellID), SpellInfo).maxStack > 0 AndAlso auraTarget.ActiveSpells(i).StackCount < (CType(SPELLs(SpellID), SpellInfo).maxStack - 1) Then
                                    auraTarget.ActiveSpells(i).StackCount += 1
                                    auraTarget.SetAuraStackCount(i, auraTarget.ActiveSpells(i).StackCount)
                                    If TypeOf auraTarget Is CharacterObject Then
                                        Dim AuraFlag_Slot As Integer = i \ 4
                                        CType(auraTarget, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_AURAAPPLICATIONS + AuraFlag_Slot, auraTarget.ActiveSpells_Count(AuraFlag_Slot))
                                        CType(auraTarget, CharacterObject).SendCharacterUpdate()
                                    End If
                                End If
                            End If
                            Return SpellFailedReason.CAST_NO_ERROR
                        Else
                            If auraTarget.ActiveSpells(i).Aura1 Is Nothing Then
                                auraTarget.ActiveSpells(i).Aura1 = AURAs(SpellInfo.ApplyAuraIndex)
                                auraTarget.ActiveSpells(i).Aura1_Info = SpellInfo
                                Log.WriteLine(LogType.DEBUG, "APPLYING AURA - {0} - {1}", SpellInfo.ApplyAuraIndex, AURAs(SpellInfo.ApplyAuraIndex).Method.Name)
                                Exit For
                            ElseIf auraTarget.ActiveSpells(i).Aura2 Is Nothing Then
                                auraTarget.ActiveSpells(i).Aura2 = AURAs(SpellInfo.ApplyAuraIndex)
                                auraTarget.ActiveSpells(i).Aura2_Info = SpellInfo
                                Log.WriteLine(LogType.DEBUG, "APPLYING AURA - {0} - {1}", SpellInfo.ApplyAuraIndex, AURAs(SpellInfo.ApplyAuraIndex).Method.Name)
                                Exit For
                            ElseIf auraTarget.ActiveSpells(i).Aura3 Is Nothing Then
                                auraTarget.ActiveSpells(i).Aura3 = AURAs(SpellInfo.ApplyAuraIndex)
                                auraTarget.ActiveSpells(i).Aura3_Info = SpellInfo
                                Log.WriteLine(LogType.DEBUG, "APPLYING AURA - {0} - {1}", SpellInfo.ApplyAuraIndex, AURAs(SpellInfo.ApplyAuraIndex).Method.Name)
                                Exit For
                            Else
                                spellCasted = SLOT_NO_SPACE
                            End If
                        End If
                    End If
                Next

                'DONE: Not found same active aura on that player, create new
                If spellCasted = SLOT_NOT_FOUND Then auraTarget.AddAura(SpellID, Duration, Caster)
                If spellCasted = SLOT_CREATE_NEW Then spellCasted = SLOT_NO_SPACE
                If spellCasted < 0 Then spellCasted -= 1
            Loop While spellCasted < 0

            'DONE: No more space for auras
            If spellCasted = SLOT_NO_SPACE Then Return SpellFailedReason.CAST_FAIL_TRY_AGAIN

            'DONE: Cast the aura
            AURAs(SpellInfo.ApplyAuraIndex).Invoke(auraTarget, Caster, SpellInfo, SpellID, 1, AuraAction.AURA_ADD)

        Catch e As Exception
            Log.WriteLine(LogType.CRITICAL, "Error while applying aura for spell {0}:{1}", SpellID, vbNewLine & e.ToString)
        End Try

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_APPLY_AURA(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If ((Target.targetMask And SpellCastTargetFlags.TARGET_FLAG_UNIT) OrElse Target.targetMask = SpellCastTargetFlags.TARGET_FLAG_SELF) AndAlso Target.unitTarget Is Nothing Then Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS

        Dim result As SpellFailedReason = SpellFailedReason.CAST_NO_ERROR

        If (Target.targetMask And SpellCastTargetFlags.TARGET_FLAG_UNIT) OrElse Target.targetMask = SpellCastTargetFlags.TARGET_FLAG_SELF Then
            Dim auraTarget As BaseUnit = Target.unitTarget
            Dim auraTargets As New List(Of BaseUnit)

            Select Case SpellInfo.implicitTargetA
                Case SpellImplicitTargets.TARGET_SELF
                    auraTarget = Caster
                Case SpellImplicitTargets.TARGET_DUEL_VS_PLAYER
                    CType(Caster, CharacterObject).DuelArbiter = Target.unitTarget.GUID
                Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                    auraTargets = GetEnemyAroundMe(Caster, SpellInfo.GetRadius)
                Case SpellImplicitTargets.TARGET_AROUND_CASTER_PARTY
                    auraTargets = GetPartyMembersAroundMe(Caster, SpellInfo.GetRadius)
            End Select

            If auraTargets.Count = 0 Then
                'DONE: Sit down on some spells
                If TypeOf Caster Is CharacterObject AndAlso (SPELLs(SpellID).auraInterruptFlags And SpellAuraInterruptFlags.AURA_INTERRUPT_FLAG_NOT_SEATED) Then
                    Caster.StandState = StandStates.STANDSTATE_SIT
                    CType(Caster, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_BYTES_1, Caster.cBytes1)
                    CType(Caster, CharacterObject).SendCharacterUpdate(True)
                    Dim packetACK As New PacketClass(OPCODES.SMSG_STANDSTATE_UPDATE)
                    packetACK.AddInt8(Caster.StandState)
                    CType(Caster, CharacterObject).Client.Send(packetACK)
                    packetACK.Dispose()
                End If

                result = ApplyAura(auraTarget, Caster, SpellInfo, SpellID)
                Infected.Add(auraTarget.GUID)
            Else
                For Each u As BaseUnit In auraTargets
                    ApplyAura(u, Caster, SpellInfo, SpellID)
                    Infected.Add(u.GUID)
                Next
            End If

        ElseIf (Target.targetMask And SpellCastTargetFlags.TARGET_FLAG_DEST_LOCATION) Then
            For Each dynamic As DynamicObjectObject In Caster.dynamicObjects.ToArray
                If dynamic.SpellID = SpellID Then
                    dynamic.AddEffect(SpellInfo)
                    Exit For
                End If
            Next
        End If

        Return result
    End Function

    Public Function SPELL_EFFECT_APPLY_AREA_AURA(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If Target.unitTarget Is Nothing Then Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS

        Dim result As SpellFailedReason = SpellFailedReason.CAST_NO_ERROR
        Dim auraTarget As BaseUnit = Target.unitTarget
        Dim auraTargets As New List(Of BaseUnit)

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELF
                'DONE: Spell cast to target, but aura to caster (Mind Vision)
                auraTarget = Caster
            Case SpellImplicitTargets.TARGET_DUEL_VS_PLAYER
                CType(Caster, CharacterObject).DuelArbiter = Target.unitTarget.GUID
            Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                auraTargets = GetEnemyAroundMe(Caster, SpellInfo.GetRadius)
            Case SpellImplicitTargets.TARGET_AROUND_CASTER_PARTY
                auraTargets = GetPartyMembersAroundMe(Caster, SpellInfo.GetRadius)
        End Select

        If auraTargets.Count = 0 Then
            result = ApplyAura(auraTarget, Caster, SpellInfo, SpellID)
            Infected.Add(auraTarget.GUID)
        Else
            For Each u As BaseUnit In auraTargets
                ApplyAura(u, Caster, SpellInfo, SpellID)
                Infected.Add(u.GUID)
            Next
        End If

        Return result
    End Function

    Public Function SPELL_EFFECT_PERSISTENT_AREA_AURA(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If (Target.targetMask And SpellCastTargetFlags.TARGET_FLAG_DEST_LOCATION) = 0 Then Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS

        Log.WriteLine(LogType.DEBUG, "Amplitude: {0}", SpellInfo.Amplitude)
        Dim tmpDO As New DynamicObjectObject(Caster, SpellID, Target.dstX, Target.dstY, Target.dstZ, SPELLs(SpellID).GetDuration, SpellInfo.GetRadius)
        tmpDO.AddEffect(SpellInfo)
        tmpDO.Bytes = &H1EEEEEE
        Caster.dynamicObjects.Add(tmpDO)
        tmpDO.Spawn()

        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function SPELL_EFFECT_CREATE_ITEM(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If Not TypeOf Target.unitTarget Is CharacterObject Then Return SpellFailedReason.CAST_FAIL_BAD_TARGETS
        Dim Amount As Integer = SpellInfo.GetValue(Caster.Level - SPELLs(SpellID).spellLevel)
        If Amount < 0 Then Return SpellFailedReason.CAST_FAIL_ERROR
        If ITEMDatabase.ContainsKey(SpellInfo.ItemType) = False Then Dim tmpInfo As New ItemInfo(SpellInfo.ItemType)
        If Amount > ITEMDatabase(SpellInfo.ItemType).Stackable Then Amount = ITEMDatabase(SpellInfo.ItemType).Stackable

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELECTED_PLAYER
                Dim tmpItem As New ItemObject(SpellInfo.ItemType, Target.unitTarget.GUID)
                tmpItem.StackCount = Amount
                If Not CType(Target.unitTarget, CharacterObject).ItemADD(tmpItem) Then
                    tmpItem.Delete()
                    SendInventoryChangeFailure(CType(Target.unitTarget, CharacterObject), InventoryChangeFailure.EQUIP_ERR_INVENTORY_FULL, 0, 0)
                    Return SpellFailedReason.CAST_FAIL_TRY_AGAIN
                Else
                    CType(Target.unitTarget, CharacterObject).LogLootItem(tmpItem, tmpItem.StackCount, False, True)
                End If
                Infected.Add(Target.unitTarget.GUID)

            Case SpellImplicitTargets.TARGET_SELECTED_FRIEND, SpellImplicitTargets.TARGET_DUEL_VS_PLAYER
                Dim tmpItem As New ItemObject(SpellInfo.ItemType, Target.unitTarget.GUID)
                tmpItem.StackCount = Amount
                If Not CType(Target.unitTarget, CharacterObject).ItemADD(tmpItem) Then
                    tmpItem.Delete()
                    SendInventoryChangeFailure(CType(Target.unitTarget, CharacterObject), InventoryChangeFailure.EQUIP_ERR_INVENTORY_FULL, 0, 0)
                    Return SpellFailedReason.CAST_FAIL_TRY_AGAIN
                Else
                    CType(Target.unitTarget, CharacterObject).LogLootItem(tmpItem, tmpItem.StackCount, False, True)
                End If
                Infected.Add(Target.unitTarget.GUID)

            Case SpellImplicitTargets.TARGET_SELF, SpellImplicitTargets.TARGET_SELECTED_ENEMY
                Dim tmpItem As New ItemObject(SpellInfo.ItemType, Caster.GUID)
                tmpItem.StackCount = Amount
                If Not CType(Caster, CharacterObject).ItemADD(tmpItem) Then
                    tmpItem.Delete()
                    SendInventoryChangeFailure(CType(Target.unitTarget, CharacterObject), InventoryChangeFailure.EQUIP_ERR_INVENTORY_FULL, 0, 0)
                    Return SpellFailedReason.CAST_FAIL_TRY_AGAIN
                Else
                    CType(Target.unitTarget, CharacterObject).LogLootItem(tmpItem, tmpItem.StackCount, False, True)
                End If
                Infected.Add(Caster.GUID)

            Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                Dim Targets As List(Of BaseUnit) = GetFriendPlayersAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    Infected.Add(unit.GUID)
                    Dim tmpItem As New ItemObject(SpellInfo.ItemType, unit.GUID)
                    tmpItem.StackCount = Amount
                    If Not CType(unit, CharacterObject).ItemADD(tmpItem) Then
                        tmpItem.Delete()
                    Else
                        CType(Target.unitTarget, CharacterObject).LogLootItem(tmpItem, tmpItem.StackCount, False, True)
                    End If
                Next

            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_RESURRECT(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_NOTHING
                Target.unitTarget.Life.Current = Target.unitTarget.Life.Maximum * SpellInfo.valueBase \ 100
                If TypeOf Target.unitTarget Is CharacterObject Then CharacterResurrect(CType(Target.unitTarget, CharacterObject))
                Infected.Add(Target.unitTarget.GUID)
            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_TELEPORT_GRAVEYARD(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_NOTHING
                If TypeOf Target.unitTarget Is CharacterObject Then
                    GoToNearestGraveyard(CType(Target.unitTarget, CharacterObject))
                    Infected.Add(Target.unitTarget.GUID)
                End If
            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function SPELL_EFFECT_INTERRUPT_CAST(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        If TypeOf Target.unitTarget Is CharacterObject Then
            If CType(Target.unitTarget, CharacterObject).spellCasted = 0 Or SPELLs.ContainsKey(CType(Target.unitTarget, CharacterObject).spellCasted) = False Then Return SpellFailedReason.CAST_NO_ERROR
            CType(Target.unitTarget, CharacterObject).spellCastState = SpellCastState.SPELL_STATE_IDLE
            CType(SPELLs(CType(Target.unitTarget, CharacterObject).spellCasted), SpellInfo).SendInterrupted(0, Target.unitTarget)
            SendCastResult(SpellFailedReason.CAST_FAIL_INTERRUPTED, CType(Target.unitTarget, CharacterObject).Client, CType(Target.unitTarget, CharacterObject).spellCasted, 0)
            CType(Target.unitTarget, CharacterObject).spellCasted = 0
        Else
            'TODO: Interrupt creature spells
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function SPELL_EFFECT_STEALTH(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        SetFlag(Target.unitTarget.cBytes1, 25, True)
        Target.unitTarget.Invisibility = InvisibilityLevel.INIVISIBILITY
        Target.unitTarget.Invisibility_Value = SpellInfo.GetValue(Caster.Level)

        Infected.Add(Target.unitTarget.GUID)

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_DETECT(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Target.unitTarget.CanSeeInvisibility = InvisibilityLevel.INIVISIBILITY
        Target.unitTarget.CanSeeInvisibility_Stealth = SpellInfo.GetValue(Caster.Level)

        Infected.Add(Target.unitTarget.GUID)

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_LEAP(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        'TODO: Check if so that it's not getting you inside a wall or something

        Dim selectedX As Single = Caster.positionX + Math.Cos(Caster.orientation) * SpellInfo.GetRadius
        Dim selectedY As Single = Caster.positionY + Math.Sin(Caster.orientation) * SpellInfo.GetRadius
        Dim selectedZ As Single = GetZCoord(selectedX, selectedY, Caster.positionZ, Caster.MapID)
        If Math.Abs(Caster.positionZ - selectedZ) > SpellInfo.GetRadius Then selectedZ = Caster.positionZ - SpellInfo.GetRadius
        If TypeOf Caster Is CharacterObject Then
            CType(Caster, CharacterObject).Teleport(selectedX, selectedY, selectedZ, Caster.orientation, Caster.MapID)
        Else
            CType(Caster, CreatureObject).MoveToInstant(selectedX, selectedY, selectedZ)
        End If

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_SUMMON(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Select Case SpellInfo.MiscValueB
            Case SummonType.SUMMON_TYPE_GUARDIAN, SummonType.SUMMON_TYPE_POSESSED, SummonType.SUMMON_TYPE_POSESSED2
                Log.WriteLine(LogType.DEBUG, "[DEBUG] Summon Guardian")
            Case SummonType.SUMMON_TYPE_WILD
                Dim Duration As Integer = SPELLs(SpellID).GetDuration
                Dim Type As TempSummonType = TempSummonType.TEMPSUMMON_TIMED_OR_DEAD_DESPAWN
                If Duration = 0 Then Type = TempSummonType.TEMPSUMMON_DEAD_DESPAWN

                Dim SelectedX As Single, SelectedY As Single, SelectedZ As Single
                If (Target.targetMask And SpellCastTargetFlags.TARGET_FLAG_DEST_LOCATION) Then
                    SelectedX = Target.dstX
                    SelectedY = Target.dstY
                    SelectedZ = Target.dstZ
                Else
                    SelectedX = Caster.positionX
                    SelectedY = Caster.positionY
                    SelectedZ = Caster.positionZ
                End If

                Dim tmpCreature As New CreatureObject(SpellInfo.MiscValue, SelectedX, SelectedY, SelectedZ, Caster.orientation, Caster.MapID, Duration)
                'TODO: Level by engineering skill level
                tmpCreature.Level = Caster.Level
                tmpCreature.CreatedBy = Caster.GUID
                tmpCreature.CreatedBySpell = SpellID
                tmpCreature.AddToWorld()
            Case SummonType.SUMMON_TYPE_DEMON
                Log.WriteLine(LogType.DEBUG, "[DEBUG] Summon Demon")
            Case SummonType.SUMMON_TYPE_SUMMON
                Log.WriteLine(LogType.DEBUG, "[DEBUG] Summon")
            Case SummonType.SUMMON_TYPE_CRITTER, SummonType.SUMMON_TYPE_CRITTER2
                If CType(Caster, CharacterObject).OldCritter IsNot Nothing AndAlso CType(Caster, CharacterObject).OldCritter.ID = SpellInfo.MiscValue Then
                    CType(Caster, CharacterObject).OldCritter.Destroy()
                    CType(Caster, CharacterObject).OldCritter = Nothing
                    Return SpellFailedReason.CAST_NO_ERROR
                End If
                If CType(Caster, CharacterObject).OldCritter IsNot Nothing Then
                    CType(Caster, CharacterObject).OldCritter.Destroy()
                End If

                Dim SelectedX As Single, SelectedY As Single, SelectedZ As Single
                If (Target.targetMask And SpellCastTargetFlags.TARGET_FLAG_DEST_LOCATION) Then
                    SelectedX = Target.dstX
                    SelectedY = Target.dstY
                    SelectedZ = Target.dstZ
                Else
                    SelectedX = Caster.positionX
                    SelectedY = Caster.positionY
                    SelectedZ = Caster.positionZ
                End If
                CType(Caster, CharacterObject).OldCritter = New CreatureObject(SpellInfo.MiscValue, SelectedX, SelectedY, SelectedZ, Caster.orientation, Caster.MapID, SPELLs(SpellID).GetDuration)
                CType(Caster, CharacterObject).OldCritter.SummonedBy = Caster.GUID
                CType(Caster, CharacterObject).OldCritter.CreatedBy = Caster.GUID
                CType(Caster, CharacterObject).OldCritter.CreatedBySpell = SpellID
                CType(Caster, CharacterObject).OldCritter.Faction = CType(Caster, CharacterObject).Faction
                CType(Caster, CharacterObject).OldCritter.Level = 1
                CType(Caster, CharacterObject).OldCritter.Life.Base = 1
                CType(Caster, CharacterObject).OldCritter.Life.Current = 1
                CType(Caster, CharacterObject).OldCritter.AddToWorld()

            Case SummonType.SUMMON_TYPE_TOTEM, SummonType.SUMMON_TYPE_TOTEM_SLOT1, SummonType.SUMMON_TYPE_TOTEM_SLOT2, SummonType.SUMMON_TYPE_TOTEM_SLOT3, SummonType.SUMMON_TYPE_TOTEM_SLOT4
                Log.WriteLine(LogType.DEBUG, "[DEBUG] Summon Totem")
            Case Else
                Log.WriteLine(LogType.DEBUG, "Unknown SummonType: {0}", SpellInfo.MiscValueB)
                Exit Function
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_SUMMON_OBJECT(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason
        Dim selectedX As Single = Caster.positionX + Math.Cos(Caster.orientation) * SpellInfo.GetRadius
        Dim selectedY As Single = Caster.positionY + Math.Sin(Caster.orientation) * SpellInfo.GetRadius
        Dim selectedZ As Single = GetZCoord(selectedX, selectedY, Caster.positionZ, Caster.MapID)
        Dim tmpGO As New GameObjectObject(SpellInfo.MiscValue, Caster.MapID, selectedX, selectedY, selectedZ, 0, Caster.GUID)
        tmpGO.AddToWorld()

        Dim packet As New PacketClass(OPCODES.SMSG_GAMEOBJECT_SPAWN_ANIM_OBSOLETE)
        packet.AddUInt64(tmpGO.GUID)
        tmpGO.SendToNearPlayers(packet)
        packet.Dispose()

        'TODO: Expire time for objects like mage portals? could it possibly be the duration of the spell?

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_DUEL(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_DUEL_VS_PLAYER
                If Not TypeOf Target.unitTarget Is CharacterObject Then Return SpellFailedReason.CAST_FAIL_TARGET_NOT_PLAYER
                If Not TypeOf Caster Is CharacterObject Then Exit Function

                'TODO: Some more checks
                If CType(Caster, CharacterObject).DuelArbiter <> 0 Then Return SpellFailedReason.CAST_FAIL_SPELL_IN_PROGRESS
                If CType(Target.unitTarget, CharacterObject).IsInDuel Then Return SpellFailedReason.CAST_FAIL_TARGET_DUELING
                If Caster.Invisibility <> InvisibilityLevel.VISIBLE Then Return SpellFailedReason.CAST_FAIL_CANT_DUEL_WHILE_INVISIBLE
                'CAST_FAIL_CANT_START_DUEL_INVISIBLE
                'CAST_FAIL_CANT_START_DUEL_STEALTHED
                'CAST_FAIL_NO_DUELING_HERE

                'DONE: Get middle coordinate
                Dim flagX As Single = Caster.positionX + (Target.unitTarget.positionX - Caster.positionX) / 2
                Dim flagY As Single = Caster.positionY + (Target.unitTarget.positionY - Caster.positionY) / 2
                Dim flagZ As Single = GetZCoord(flagX, flagY, Caster.MapID)

                'DONE: Spawn duel flag (GO Entry in SpellInfo.MiscValue) in middle of the 2 players
                Dim tmpGO As GameObjectObject = New GameObjectObject(SpellInfo.MiscValue, Caster.MapID, flagX, flagY, flagZ, 0, Caster.GUID)
                tmpGO.AddToWorld()

                'DONE: Set duel arbiter and parner
                'CType(Caster, CharacterObject).DuelArbiter = tmpGO.GUID        Commented to fix 2 packets for duel accept
                CType(Target.unitTarget, CharacterObject).DuelArbiter = tmpGO.GUID
                CType(Caster, CharacterObject).DuelPartner = CType(Target.unitTarget, CharacterObject)
                CType(Target.unitTarget, CharacterObject).DuelPartner = CType(Caster, CharacterObject)

                'DONE: Send duel request packet
                Dim packet As New PacketClass(OPCODES.SMSG_DUEL_REQUESTED)
                packet.AddUInt64(tmpGO.GUID)
                packet.AddUInt64(Caster.GUID)
                CType(Target.unitTarget, CharacterObject).Client.SendMultiplyPackets(packet)
                CType(Caster, CharacterObject).Client.SendMultiplyPackets(packet)
                packet.Dispose()

                Infected.Add(Target.unitTarget.GUID)
            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function
    Public Function SPELL_EFFECT_QUEST_COMPLETE(ByRef Target As SpellTargets, ByRef Caster As BaseUnit, ByRef SpellInfo As SpellEffect, ByVal SpellID As Integer, ByRef Infected As ArrayList, ByRef Item As ItemObject) As SpellFailedReason

        Select Case SpellInfo.implicitTargetA
            Case SpellImplicitTargets.TARGET_SELF, SpellImplicitTargets.TARGET_SELECTED_FRIEND, SpellImplicitTargets.TARGET_SELECTED_ENEMY
                CompleteQuest(CType(Target.unitTarget, CharacterObject), SpellInfo.MiscValue, Caster.GUID)
                Infected.Add(Target.unitTarget.GUID)
            Case SpellImplicitTargets.TARGET_SELECTED_GAMEOBJECT
                CompleteQuest(CType(Caster, CharacterObject), SpellInfo.MiscValue, Target.unitTarget.GUID)
                Infected.Add(Caster.GUID)
            Case SpellImplicitTargets.TARGET_AROUND_CASTER_ENEMY
                Dim Targets As List(Of BaseUnit) = GetEnemyAroundMe(Caster, SpellInfo.GetRadius)
                For Each unit As BaseUnit In Targets
                    If TypeOf unit Is CharacterObject Then
                        CompleteQuest(CType(unit, CharacterObject), SpellInfo.MiscValue, Caster.GUID)
                        Infected.Add(unit.GUID)
                    End If
                Next
            Case Else
                Return SpellFailedReason.CAST_FAIL_BAD_IMPLICIT_TARGETS
        End Select

        Return SpellFailedReason.CAST_NO_ERROR
    End Function

    Public Function GetEnemyAtPoint(ByRef c As BaseUnit, ByVal PosX As Single, ByVal PosY As Single, ByVal PosZ As Single, ByVal Distance As Single) As List(Of BaseUnit)
        Dim result As New List(Of BaseUnit)

        If TypeOf c Is CharacterObject Then
            For Each pGUID As ULong In CType(c, CharacterObject).playersNear.ToArray
                If CHARACTERs.ContainsKey(pGUID) AndAlso (CType(c, CharacterObject).Side <> CHARACTERs(pGUID).Side OrElse (CType(c, CharacterObject).DuelPartner IsNot Nothing AndAlso CType(c, CharacterObject).DuelPartner Is CHARACTERs(pGUID))) AndAlso CHARACTERs(pGUID).DEAD = False Then
                    If GetDistance(CHARACTERs(pGUID), PosX, PosY, PosZ) < Distance Then result.Add(CHARACTERs(pGUID))
                End If
            Next

            For Each cGUID As ULong In CType(c, CharacterObject).creaturesNear.ToArray
                If WORLD_CREATUREs.ContainsKey(cGUID) AndAlso WORLD_CREATUREs(cGUID).Life.Current >= 0 AndAlso CType(c, CharacterObject).GetReaction(WORLD_CREATUREs(cGUID).Faction) <= TReaction.NEUTRAL Then
                    If GetDistance(WORLD_CREATUREs(cGUID), PosX, PosY, PosZ) < Distance Then result.Add(WORLD_CREATUREs(cGUID))
                End If
            Next

        ElseIf TypeOf c Is CreatureObject Then
            For Each pGUID As ULong In c.SeenBy.ToArray
                If CHARACTERs.ContainsKey(pGUID) AndAlso CHARACTERs(pGUID).DEAD = False AndAlso CHARACTERs(pGUID).GetReaction(CType(c, CreatureObject).Faction) <= TReaction.NEUTRAL Then
                    If GetDistance(CHARACTERs(pGUID), PosX, PosY, PosZ) < Distance Then result.Add(CHARACTERs(pGUID))
                End If
            Next
        End If

        Return result
    End Function
    Public Function GetEnemyAroundMe(ByRef c As BaseUnit, ByVal Distance As Integer) As List(Of BaseUnit)
        Dim result As New List(Of BaseUnit)

        If TypeOf c Is CharacterObject Then
            For Each pGUID As ULong In CType(c, CharacterObject).playersNear.ToArray
                If CHARACTERs.ContainsKey(pGUID) AndAlso (CType(c, CharacterObject).Side <> CHARACTERs(pGUID).Side OrElse (CType(c, CharacterObject).DuelPartner IsNot Nothing AndAlso CType(c, CharacterObject).DuelPartner Is CHARACTERs(pGUID))) AndAlso CHARACTERs(pGUID).DEAD = False Then
                    If GetDistance(CHARACTERs(pGUID), c) < Distance Then result.Add(CHARACTERs(pGUID))
                End If
            Next

            For Each cGUID As ULong In CType(c, CharacterObject).creaturesNear.ToArray
                If WORLD_CREATUREs.ContainsKey(cGUID) AndAlso WORLD_CREATUREs(cGUID).Life.Current >= 0 AndAlso CType(c, CharacterObject).GetReaction(WORLD_CREATUREs(cGUID).Faction) <= TReaction.NEUTRAL Then
                    If GetDistance(WORLD_CREATUREs(cGUID), c) < Distance Then result.Add(WORLD_CREATUREs(cGUID))
                End If
            Next

        ElseIf TypeOf c Is CreatureObject Then
            For Each pGUID As ULong In c.SeenBy.ToArray
                If CHARACTERs.ContainsKey(pGUID) AndAlso CHARACTERs(pGUID).DEAD = False AndAlso CHARACTERs(pGUID).GetReaction(CType(c, CreatureObject).Faction) <= TReaction.NEUTRAL Then
                    If GetDistance(CHARACTERs(pGUID), c) < Distance Then result.Add(CHARACTERs(pGUID))
                End If
            Next
        End If

        Return result
    End Function
    Public Function GetFriendAroundMe(ByRef c As BaseUnit, ByVal Distance As Integer) As List(Of BaseUnit)
        Dim result As New List(Of BaseUnit)

        If TypeOf c Is CharacterObject Then
            For Each pGUID As ULong In CType(c, CharacterObject).playersNear.ToArray
                If CHARACTERs.ContainsKey(pGUID) AndAlso CType(c, CharacterObject).Side = CHARACTERs(pGUID).Side AndAlso CHARACTERs(pGUID).DEAD = False Then
                    If GetDistance(CHARACTERs(pGUID), c) < Distance Then result.Add(CHARACTERs(pGUID))
                End If
            Next

            For Each cGUID As ULong In CType(c, CharacterObject).creaturesNear.ToArray
                If WORLD_CREATUREs.ContainsKey(cGUID) AndAlso WORLD_CREATUREs(cGUID).Life.Current >= 0 AndAlso CType(c, CharacterObject).GetReaction(WORLD_CREATUREs(cGUID).Faction) > TReaction.NEUTRAL Then
                    If GetDistance(WORLD_CREATUREs(cGUID), c) < Distance Then result.Add(WORLD_CREATUREs(cGUID))
                End If
            Next

        ElseIf TypeOf c Is CreatureObject Then
            For Each pGUID As ULong In c.SeenBy.ToArray
                If CHARACTERs.ContainsKey(pGUID) AndAlso CHARACTERs(pGUID).DEAD = False AndAlso CHARACTERs(pGUID).GetReaction(CType(c, CreatureObject).Faction) > TReaction.NEUTRAL Then
                    If GetDistance(CHARACTERs(pGUID), c) < Distance Then result.Add(CHARACTERs(pGUID))
                End If
            Next
        End If

        Return result
    End Function
    Public Function GetFriendPlayersAroundMe(ByRef c As BaseUnit, ByVal Distance As Integer) As List(Of BaseUnit)
        Dim result As New List(Of BaseUnit)
        Dim index As Integer = 0

        If TypeOf c Is CharacterObject Then
            For Each pGUID As ULong In CType(c, CharacterObject).playersNear.ToArray
                If CHARACTERs.ContainsKey(pGUID) AndAlso CType(c, CharacterObject).Side = CHARACTERs(pGUID).Side AndAlso CHARACTERs(pGUID).DEAD = False Then
                    If GetDistance(CHARACTERs(pGUID), c) < Distance Then result.Add(CHARACTERs(pGUID))
                End If
            Next

        ElseIf TypeOf c Is CreatureObject Then
            For Each pGUID As ULong In c.SeenBy.ToArray
                If CHARACTERs.ContainsKey(pGUID) AndAlso CHARACTERs(pGUID).DEAD = False AndAlso CHARACTERs(pGUID).GetReaction(CType(c, CreatureObject).Faction) > TReaction.NEUTRAL Then
                    If GetDistance(CHARACTERs(pGUID), c) < Distance Then result.Add(CHARACTERs(pGUID))
                End If
            Next
        End If

        Return result
    End Function
    Public Function GetPartyMembersAroundMe(ByRef c As CharacterObject, ByVal Distance As Integer) As List(Of BaseUnit)
        Dim result As New List(Of BaseUnit)
        Dim index As Integer = 0

        If Not c.IsInGroup Then Return result

        For Each GUID As ULong In c.Group.LocalMembers.ToArray
            If c.playersNear.Contains(GUID) Then
                If GetDistance(c, CHARACTERs(GUID)) < Distance Then result.Add(CHARACTERs(GUID))
            End If
        Next

        Return result
    End Function

    Public Function GetEnemyInFrontOfMe(ByRef c As BaseUnit, ByVal Distance As Integer) As List(Of BaseUnit)
        Dim result As New List(Of BaseUnit)

        Return result
    End Function

#End Region
#Region "WS.Spells.SpellAuraEffects"

    Public Enum AuraAction As Byte
        AURA_ADD
        AURA_UPDATE
        AURA_REMOVE
    End Enum

    Delegate Sub ApplyAuraHandler(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
    Public Const AURAs_COUNT As Integer = 261
    Public AURAs(AURAs_COUNT) As ApplyAuraHandler

    Public Sub SPELL_AURA_NONE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

    End Sub
    Public Sub SPELL_AURA_DUMMY(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        Log.WriteLine(LogType.DEBUG, "[DEBUG] Aura Dummy for spell {0}.", SpellID)
        Select Case Action
            Case AuraAction.AURA_REMOVE
                Select Case SpellID
                    Case 33763
                        'HACK: Lifebloom
                        'TODO: Can lifebloom crit (the end damage)?
                        Dim Damage As Integer = EffectInfo.GetValue(Caster.Level)
                        SendHealSpellLog(Caster, Target, SpellID, Damage, False)
                        Target.Heal(Damage)
                End Select
        End Select
    End Sub
    Public Sub SPELL_AURA_BIND_SIGHT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If TypeOf Caster Is CharacterObject Then
                    CType(Caster, CharacterObject).DuelArbiter = Target.GUID
                    CType(Caster, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FARSIGHT, Target.GUID)
                    CType(Caster, CharacterObject).SendCharacterUpdate(True)
                End If

            Case AuraAction.AURA_REMOVE
                If TypeOf Caster Is CharacterObject Then
                    CType(Caster, CharacterObject).DuelArbiter = 0
                    CType(Caster, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FARSIGHT, CType(0, Long))
                    CType(Caster, CharacterObject).SendCharacterUpdate(True)
                End If
        End Select

    End Sub
    Public Sub SPELL_AURA_FAR_SIGHT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FARSIGHT, EffectInfo.MiscValue)
                    CType(Target, CharacterObject).SendCharacterUpdate(True)
                End If

            Case AuraAction.AURA_REMOVE
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FARSIGHT, 0)
                    CType(Target, CharacterObject).SendCharacterUpdate(True)
                End If
        End Select

    End Sub
    Public Sub SPELL_AURA_MECHANIC_IMMUNITY(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.RemoveAurasByMechanic(EffectInfo.MiscValue)

            Case AuraAction.AURA_REMOVE
                Exit Sub
        End Select

    End Sub
    Public Sub SPELL_AURA_TRACK_CREATURES(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_TRACK_CREATURES, 1 << (EffectInfo.MiscValue - 1))
                    CType(Target, CharacterObject).SendCharacterUpdate(True)
                End If

            Case AuraAction.AURA_REMOVE
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_TRACK_CREATURES, 0)
                    CType(Target, CharacterObject).SendCharacterUpdate(True)
                End If
        End Select

    End Sub
    Public Sub SPELL_AURA_TRACK_RESOURCES(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_TRACK_RESOURCES, 1 << (EffectInfo.MiscValue - 1))
                    CType(Target, CharacterObject).SendCharacterUpdate(True)
                End If

            Case AuraAction.AURA_REMOVE
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_TRACK_RESOURCES, 0)
                    CType(Target, CharacterObject).SendCharacterUpdate(True)
                End If
        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_SCALE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub
            Case AuraAction.AURA_ADD
                Target.Size *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
            Case AuraAction.AURA_REMOVE
                Target.Size /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
        End Select

        'DONE: Send update
        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EObjectFields.OBJECT_FIELD_SCALE_X, Target.Size)
            CType(Target, CharacterObject).SendCharacterUpdate(True)
        Else
            Dim packet As New UpdatePacketClass
            Dim tmpUpdate As New UpdateClass(EObjectFields.OBJECT_END)
            tmpUpdate.SetUpdateFlag(EObjectFields.OBJECT_FIELD_SCALE_X, Target.Size)
            tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
            Target.SendToNearPlayers(CType(packet, UpdatePacketClass))
            tmpUpdate.Dispose()
            packet.Dispose()
        End If

    End Sub
    Public Sub SPELL_AURA_MOD_SKILL(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If TypeOf Target Is CharacterObject AndAlso CType(Target, CharacterObject).Skills.ContainsKey(EffectInfo.MiscValue) Then
                    With CType(Target, CharacterObject)
                        .Skills(EffectInfo.MiscValue).Bonus += EffectInfo.GetValue(Caster.Level)
                        .SetUpdateFlag(EPlayerFields.PLAYER_SKILL_INFO_1_1 + .SkillsPositions(EffectInfo.MiscValue) * 3 + 2, CType(.Skills(EffectInfo.MiscValue), TSkill).Bonus)                      'skill1.Bonus
                        .SendCharacterUpdate(True)
                    End With
                End If

            Case AuraAction.AURA_REMOVE
                If TypeOf Target Is CharacterObject AndAlso CType(Target, CharacterObject).Skills.ContainsKey(EffectInfo.MiscValue) Then
                    With CType(Target, CharacterObject)
                        .Skills(EffectInfo.MiscValue).Bonus -= EffectInfo.GetValue(Caster.Level)
                        .SetUpdateFlag(EPlayerFields.PLAYER_SKILL_INFO_1_1 + .SkillsPositions(EffectInfo.MiscValue) * 3 + 2, CType(.Skills(EffectInfo.MiscValue), TSkill).Bonus)                      'skill1.Bonus
                        .SendCharacterUpdate(True)
                    End With
                End If
        End Select
    End Sub

    Public Sub SPELL_AURA_PERIODIC_DUMMY(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                If Not TypeOf Caster Is CharacterObject Then Exit Sub
                Select Case SpellID
                    Case 430, 431, 432, 1133, 1135, 1137, 10250, 22734, 27089, 34291, 43706, 46755
                        'HACK: Drink
                        Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                        CType(Caster, CharacterObject).ManaRegenBonus += Damage
                        CType(Caster, CharacterObject).UpdateManaRegen()
                End Select
            Case AuraAction.AURA_REMOVE
                If Not TypeOf Caster Is CharacterObject Then Exit Sub
                Select Case SpellID
                    Case 430, 431, 432, 1133, 1135, 1137, 10250, 22734, 27089, 34291, 43706, 46755
                        'HACK: Drink
                        Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                        CType(Caster, CharacterObject).ManaRegenBonus -= Damage
                        CType(Caster, CharacterObject).UpdateManaRegen()
                End Select
            Case AuraAction.AURA_UPDATE
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount

                Log.WriteLine(LogType.DEBUG, "[DEBUG] Unhandled periodic dummy: {0}", SpellID)
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_DAMAGE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
            Case AuraAction.AURA_UPDATE
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                SendPeriodicAuraLog(Caster, Target, SpellID, Damage, 0, 0, EffectInfo.ApplyAuraIndex)
                Target.DealDamage(Damage, Caster)
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_HEAL(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
            Case AuraAction.AURA_UPDATE
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                SendPeriodicHealAuraLog(Caster, Target, SpellID, Damage, EffectInfo.ApplyAuraIndex)
                Target.Heal(Damage, Caster)
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_ENERGIZE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
            Case AuraAction.AURA_UPDATE
                Dim Power As ManaTypes = EffectInfo.MiscValue
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                SendPeriodicEnergizeAuraLog(Caster, Target, SpellID, Power, Damage, EffectInfo.ApplyAuraIndex)
                Target.Energize(Damage, Power, Caster)
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_LEECH(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
            Case AuraAction.AURA_UPDATE
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                SendPeriodicAuraLog(Caster, Target, SpellID, Damage, 0, 0, EffectInfo.ApplyAuraIndex)
                SendPeriodicHealAuraLog(Target, Caster, SpellID, Damage, EffectInfo.ApplyAuraIndex)
                Target.DealDamage(Damage, Caster)
                Caster.Heal(Damage, Target)
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_MANA_LEECH(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
            Case AuraAction.AURA_UPDATE
                Dim Power As ManaTypes = EffectInfo.MiscValue
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                SendPeriodicManaLeechAuraLog(Target, Caster, SpellID, Power, Damage, 1, EffectInfo.ApplyAuraIndex)
                Target.Energize(-Damage, Power, Caster)
                Caster.Energize(Damage, Power, Target)
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_TRIGGER_SPELL(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
            Case AuraAction.AURA_UPDATE
                Dim Targets As New SpellTargets
                Targets.SetTarget_UNIT(Target)
                CType(SPELLs(EffectInfo.TriggerSpell), SpellInfo).Cast(Caster, Targets, 0)
                SendPeriodicAuraLog(Caster, Target, SpellID, 0, 0, 0, EffectInfo.ApplyAuraIndex)
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_DAMAGE_PERCENT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
            Case AuraAction.AURA_UPDATE
                Dim Damage As Integer = (Target.Life.Maximum * EffectInfo.GetValue(Caster.Level) / 100) * StackCount
                SendPeriodicAuraLog(Caster, Target, SpellID, Damage, 0, 0, EffectInfo.ApplyAuraIndex)
                Target.DealDamage(Damage, Caster)
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_HEAL_PERCENT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Dim Damage As Integer = (Target.Life.Maximum * EffectInfo.GetValue(Caster.Level) / 100) * StackCount
                SendPeriodicHealAuraLog(Caster, Target, SpellID, Damage, EffectInfo.ApplyAuraIndex)
                Target.Heal(Damage, Caster)

            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
        End Select

    End Sub
    Public Sub SPELL_AURA_PERIODIC_ENERGIZE_PERCENT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Dim Power As ManaTypes = EffectInfo.MiscValue
                Dim Damage As Integer = (Target.Mana.Maximum * EffectInfo.GetValue(Caster.Level) / 100) * StackCount
                SendPeriodicEnergizeAuraLog(Caster, Target, SpellID, Power, Damage, EffectInfo.ApplyAuraIndex)
                Target.Energize(Damage, Power, Caster)

            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_REGEN(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub
            Case AuraAction.AURA_ADD
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                CType(Target, CharacterObject).LifeRegenBonus += Damage

                'TODO: Increase threat (gain * 0.5)

                If (CType(SPELLs(SpellID), SpellInfo).auraInterruptFlags And SpellAuraInterruptFlags.AURA_INTERRUPT_FLAG_NOT_SEATED) Then
                    CType(Target, CharacterObject).StandState = StandStates.STANDSTATE_SIT
                    CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_BYTES_1, CType(Target, CharacterObject).cBytes1)
                    CType(Target, CharacterObject).SendCharacterUpdate()
                    Dim packetACK As New PacketClass(OPCODES.SMSG_STANDSTATE_UPDATE)
                    packetACK.AddInt8(StandStates.STANDSTATE_SIT)
                    CType(Target, CharacterObject).Client.Send(packetACK)
                    packetACK.Dispose()

                    'Eat emote
                    Dim SMSG_EMOTE As New PacketClass(OPCODES.SMSG_EMOTE)
                    SMSG_EMOTE.AddInt32(Emotes.ONESHOT_EAT)
                    SMSG_EMOTE.AddUInt64(Target.GUID)
                    CType(Target, CharacterObject).Client.SendMultiplyPackets(SMSG_EMOTE)
                    CType(Target, CharacterObject).SendToNearPlayers(SMSG_EMOTE)
                    SMSG_EMOTE.Dispose()
                ElseIf SpellID = 20577 Then
                    'HACK: Cannibalize emote
                    Dim SMSG_EMOTE As New PacketClass(OPCODES.SMSG_EMOTE)
                    SMSG_EMOTE.AddInt32(398)
                    SMSG_EMOTE.AddUInt64(Target.GUID)
                    CType(Target, CharacterObject).Client.SendMultiplyPackets(SMSG_EMOTE)
                    CType(Target, CharacterObject).SendToNearPlayers(SMSG_EMOTE)
                    SMSG_EMOTE.Dispose()
                End If
            Case AuraAction.AURA_REMOVE
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                CType(Target, CharacterObject).LifeRegenBonus -= Damage
        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_POWER_REGEN(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub
            Case AuraAction.AURA_ADD
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                If EffectInfo.MiscValue = ManaTypes.TYPE_MANA Then
                    CType(Target, CharacterObject).ManaRegenBonus += Damage
                    CType(Target, CharacterObject).UpdateManaRegen()
                ElseIf EffectInfo.MiscValue = ManaTypes.TYPE_RAGE Then
                    CType(Target, CharacterObject).RageRegenBonus += ((Damage / 17) * 10)
                End If

                If (CType(SPELLs(SpellID), SpellInfo).auraInterruptFlags And SpellAuraInterruptFlags.AURA_INTERRUPT_FLAG_NOT_SEATED) Then
                    'Eat emote
                    Dim SMSG_EMOTE As New PacketClass(OPCODES.SMSG_EMOTE)
                    SMSG_EMOTE.AddInt32(Emotes.ONESHOT_EAT)
                    SMSG_EMOTE.AddUInt64(Target.GUID)
                    CType(Target, CharacterObject).Client.SendMultiplyPackets(SMSG_EMOTE)
                    CType(Target, CharacterObject).SendToNearPlayers(SMSG_EMOTE)
                    SMSG_EMOTE.Dispose()
                ElseIf SpellID = 20577 Then
                    'HACK: Cannibalize emote
                    Dim SMSG_EMOTE As New PacketClass(OPCODES.SMSG_EMOTE)
                    SMSG_EMOTE.AddInt32(398)
                    SMSG_EMOTE.AddUInt64(Target.GUID)
                    CType(Target, CharacterObject).Client.SendMultiplyPackets(SMSG_EMOTE)
                    CType(Target, CharacterObject).SendToNearPlayers(SMSG_EMOTE)
                    SMSG_EMOTE.Dispose()
                End If
            Case AuraAction.AURA_REMOVE
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount
                If EffectInfo.MiscValue = ManaTypes.TYPE_MANA Then
                    CType(Target, CharacterObject).ManaRegenBonus -= Damage
                    CType(Target, CharacterObject).UpdateManaRegen()
                ElseIf EffectInfo.MiscValue = ManaTypes.TYPE_RAGE Then
                    CType(Target, CharacterObject).RageRegenBonus -= (Damage / 17) * 10
                End If
        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_POWER_REGEN_PERCENT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub
            Case AuraAction.AURA_ADD
                If EffectInfo.MiscValue = ManaTypes.TYPE_MANA Then
                    Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount / 100
                    CType(Target, CharacterObject).ManaRegenerationModifier += Damage
                    CType(Target, CharacterObject).UpdateManaRegen()
                End If

            Case AuraAction.AURA_REMOVE
                Dim Damage As Integer = EffectInfo.GetValue(Caster.Level) * StackCount / 100
                CType(Target, CharacterObject).ManaRegenerationModifier -= Damage
                CType(Target, CharacterObject).UpdateManaRegen()
        End Select

    End Sub

    Public Sub SPELL_AURA_TRANSFORM(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                If Not CREATURESDatabase.ContainsKey(EffectInfo.MiscValue) Then
                    Dim creature As New CreatureInfo(EffectInfo.MiscValue)
                End If
                Target.Model = CType(CREATURESDatabase(EffectInfo.MiscValue), CreatureInfo).Model

            Case AuraAction.AURA_REMOVE
                If TypeOf Target Is CharacterObject Then
                    Target.Model = GetRaceModel(CType(Target, CharacterObject).Race, CType(Target, CharacterObject).Gender)
                Else
                    Target.Model = CType(Target, CreatureObject).CreatureInfo.Model
                End If

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select

        'DONE: Model update
        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_DISPLAYID, Target.Model)
            CType(Target, CharacterObject).SendCharacterUpdate(True)
        Else
            Dim packet As New UpdatePacketClass
            Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
            tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_DISPLAYID, Target.Model)
            tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
            Target.SendToNearPlayers(CType(packet, UpdatePacketClass))
            tmpUpdate.Dispose()
            packet.Dispose()
        End If

    End Sub

    Public Sub SPELL_AURA_GHOST(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.Invisibility = InvisibilityLevel.DEAD
                Target.CanSeeInvisibility = InvisibilityLevel.DEAD
                UpdateCell(Target)

            Case AuraAction.AURA_REMOVE
                Target.Invisibility = InvisibilityLevel.VISIBLE
                Target.CanSeeInvisibility = InvisibilityLevel.INIVISIBILITY
                UpdateCell(Target)

        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_INVISIBILITY(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                SetFlag(Target.cBytes1, 25, True)
                Target.Invisibility = InvisibilityLevel.INIVISIBILITY
                Target.Invisibility_Value += EffectInfo.GetValue(Caster.Level)

            Case AuraAction.AURA_REMOVE
                SetFlag(Target.cBytes1, 25, False)
                Target.Invisibility = InvisibilityLevel.VISIBLE
                Target.Invisibility_Value -= EffectInfo.GetValue(Caster.Level)

        End Select

        'DONE: Send update
        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_BYTES_1, Target.cBytes1)
            CType(Target, CharacterObject).SendCharacterUpdate(True)
            UpdateCell(CType(Target, CharacterObject))
        Else
            'TODO: Still not done for creatures (we can't get their stand state!)
        End If

    End Sub
    Public Sub SPELL_AURA_MOD_STEALTH(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                SetFlag(Target.cBytes1, 25, True)
                Target.Invisibility = InvisibilityLevel.STEALTH
                Target.Invisibility_Value += EffectInfo.GetValue(Caster.Level)

            Case AuraAction.AURA_REMOVE
                SetFlag(Target.cBytes1, 25, False)
                Target.Invisibility = InvisibilityLevel.VISIBLE
                Target.Invisibility_Value -= EffectInfo.GetValue(Caster.Level)

        End Select

        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_BYTES_1, Target.cBytes1)
            CType(Target, CharacterObject).SendCharacterUpdate(True)
            UpdateCell(CType(Target, CharacterObject))
        Else
            'TODO: Still not done for creatures (we can't get their stand state!)
        End If

    End Sub
    Public Sub SPELL_AURA_MOD_INVISIBILITY_DETECTION(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.CanSeeInvisibility_Invisibility += EffectInfo.GetValue(Caster.Level)

            Case AuraAction.AURA_REMOVE
                Target.CanSeeInvisibility_Invisibility -= EffectInfo.GetValue(Caster.Level)

        End Select

        If TypeOf Target Is CharacterObject Then
            UpdateCell(CType(Target, CharacterObject))
        End If
    End Sub
    Public Sub SPELL_AURA_MOD_DETECT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.CanSeeInvisibility_Stealth += EffectInfo.GetValue(Caster.Level)

            Case AuraAction.AURA_REMOVE
                Target.CanSeeInvisibility_Stealth -= EffectInfo.GetValue(Caster.Level)

        End Select

        If TypeOf Target Is CharacterObject Then
            UpdateCell(CType(Target, CharacterObject))
        End If
    End Sub
    Public Sub SPELL_AURA_MOD_SHAPESHIFT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Target.RemoveAurasOfType(AuraEffects_Names.SPELL_AURA_MOD_SHAPESHIFT, SpellID)  'Remove other shapeshift forms
                Target.RemoveAurasOfType(AuraEffects_Names.SPELL_AURA_MOUNTED)                  'Remove mounted spells

                'Druid
                If TypeOf Target Is CharacterObject AndAlso CType(Target, CharacterObject).Classe = Classes.CLASS_DRUID Then
                    If EffectInfo.MiscValue = ShapeshiftForm.FORM_AQUA OrElse EffectInfo.MiscValue = ShapeshiftForm.FORM_CAT OrElse EffectInfo.MiscValue = ShapeshiftForm.FORM_BEAR OrElse EffectInfo.MiscValue = ShapeshiftForm.FORM_DIREBEAR OrElse EffectInfo.MiscValue = ShapeshiftForm.FORM_TRAVEL OrElse EffectInfo.MiscValue = ShapeshiftForm.FORM_FLIGHT OrElse EffectInfo.MiscValue = ShapeshiftForm.FORM_SWIFT OrElse EffectInfo.MiscValue = ShapeshiftForm.FORM_MOONKIN Then
                        Target.RemoveAurasOfType(26) 'Remove Root
                        Target.RemoveAurasOfType(33) 'Remove Slow
                    End If
                End If

                Target.ShapeshiftForm = EffectInfo.MiscValue
                Target.ManaType = GetShapeshiftManaType(EffectInfo.MiscValue, Target.ManaType)
                If TypeOf Target Is CharacterObject Then
                    Target.Model = GetShapeshiftModel(EffectInfo.MiscValue, CType(Target, CharacterObject).Race, Target.Model)
                Else
                    Target.Model = GetShapeshiftModel(EffectInfo.MiscValue, 0, Target.Model)
                End If

            Case AuraAction.AURA_REMOVE
                Target.ShapeshiftForm = ShapeshiftForm.FORM_NORMAL

                If TypeOf Target Is CharacterObject Then
                    Target.ManaType = GetClassManaType(CType(Target, CharacterObject).Classe)
                    Target.Model = GetRaceModel(CType(Target, CharacterObject).Race, CType(Target, CharacterObject).Gender)
                Else
                    Target.ManaType = CType(Target, CreatureObject).CreatureInfo.ManaType
                    Target.Model = CType(Target, CreatureObject).CreatureInfo.Model
                End If

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select

        'DONE: Send update
        If TypeOf Target Is CharacterObject Then
            With CType(Target, CharacterObject)
                .SetUpdateFlag(EUnitFields.UNIT_FIELD_BYTES_2, .cBytes2)
                .SetUpdateFlag(EUnitFields.UNIT_FIELD_DISPLAYID, .Model)
                .SetUpdateFlag(EUnitFields.UNIT_FIELD_BYTES_0, CType(CType(.Race, Integer) + (CType(.Classe, Integer) << 8) + (CType(.Gender, Integer) << 16) + (CType(.ManaType, Integer) << 24), Integer))
                If .ManaType = ManaTypes.TYPE_MANA Then
                    .SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1, .Mana.Current)
                    .SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1, .Mana.Maximum)
                ElseIf .ManaType = ManaTypes.TYPE_RAGE Then
                    .SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER2, .Rage.Current)
                    .SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER2, .Rage.Maximum)
                ElseIf .ManaType = ManaTypes.TYPE_ENERGY Then
                    .SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER4, .Energy.Current)
                    .SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER4, .Energy.Maximum)
                End If
                CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.BASE_ATTACK)
                .SetUpdateFlag(EUnitFields.UNIT_FIELD_MINDAMAGE, .Damage.Minimum)
                .SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXDAMAGE, .Damage.Maximum)
                .SendCharacterUpdate(True)
                .GroupUpdate(PartyMemberStatsFlag.GROUP_UPDATE_FLAG_POWER_TYPE)
                InitializeTalentSpells(CType(Target, CharacterObject))
            End With
        Else
            Dim packet As New UpdatePacketClass
            Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
            tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_BYTES_2, Target.cBytes2)
            tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_DISPLAYID, Target.Model)
            tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
            Target.SendToNearPlayers(CType(packet, UpdatePacketClass))
            tmpUpdate.Dispose()
            packet.Dispose()
        End If

        'TODO: The running emote is fucked up
        If TypeOf Target Is CharacterObject Then
            If Action = AuraAction.AURA_ADD Then
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_TRAVEL Then CType(Target, CharacterObject).ApplySpell(5419)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_TREE Then
                    CType(Target, CharacterObject).ApplySpell(5420)
                    CType(Target, CharacterObject).ApplySpell(34123)
                End If
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_CAT Then CType(Target, CharacterObject).ApplySpell(3025)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_BEAR Then CType(Target, CharacterObject).ApplySpell(1178)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_DIREBEAR Then CType(Target, CharacterObject).ApplySpell(9635)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_AQUA Then CType(Target, CharacterObject).ApplySpell(5421)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_MOONKIN Then CType(Target, CharacterObject).ApplySpell(24905)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_FLIGHT Then
                    CType(Target, CharacterObject).ApplySpell(33948)
                    CType(Target, CharacterObject).ApplySpell(34764)
                End If
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_SWIFT Then
                    CType(Target, CharacterObject).ApplySpell(40121)
                    CType(Target, CharacterObject).ApplySpell(40122)
                End If
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_BATTLESTANCE Then CType(Target, CharacterObject).ApplySpell(21156)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_BERSERKERSTANCE Then CType(Target, CharacterObject).ApplySpell(7381)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_DEFENSIVESTANCE Then CType(Target, CharacterObject).ApplySpell(7376)
            Else
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_TRAVEL Then Target.RemoveAuraBySpell(5419)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_TREE Then
                    'TODO: 5420 never removes it's speed
                    Target.RemoveAuraBySpell(5420)
                    Target.RemoveAuraBySpell(34123)
                End If
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_CAT Then Target.RemoveAuraBySpell(3025)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_BEAR Then Target.RemoveAuraBySpell(1178)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_DIREBEAR Then Target.RemoveAuraBySpell(9635)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_AQUA Then Target.RemoveAuraBySpell(5421)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_MOONKIN Then Target.RemoveAuraBySpell(24905)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_FLIGHT Then
                    Target.RemoveAuraBySpell(33948)
                    Target.RemoveAuraBySpell(34764)
                End If
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_SWIFT Then
                    Target.RemoveAuraBySpell(40121)
                    Target.RemoveAuraBySpell(40122)
                End If
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_BATTLESTANCE Then Target.RemoveAuraBySpell(21156)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_BERSERKERSTANCE Then Target.RemoveAuraBySpell(7381)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_DEFENSIVESTANCE Then Target.RemoveAuraBySpell(7376)
                If EffectInfo.MiscValue = ShapeshiftForm.FORM_GHOSTWOLF Then Target.RemoveAuraBySpell(7376)
            End If
        End If
    End Sub

    Public Sub SPELL_AURA_PROC_TRIGGER_SPELL(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        Select Case Action
            Case AuraAction.AURA_ADD
                Exit Sub
            Case AuraAction.AURA_REMOVE
                Exit Sub
            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub

    Public Sub SPELL_AURA_MOD_INCREASE_SPEED(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                newSpeed *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                newSpeed /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_DECREASE_SPEED(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        'NOTE: Some values of EffectInfo.GetValue are in old format, new format uses (-) values

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                If EffectInfo.GetValue(Caster.Level) < 0 Then
                    newSpeed /= Math.Abs(EffectInfo.GetValue(Caster.Level) / 100) + 1
                Else
                    newSpeed /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                End If

                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                If EffectInfo.GetValue(Caster.Level) < 0 Then
                    newSpeed *= Math.Abs(EffectInfo.GetValue(Caster.Level) / 100) + 1
                Else
                    newSpeed *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                End If
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_SPEED_ALWAYS(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                newSpeed *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                newSpeed /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_MOUNTED_SPEED(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                newSpeed *= ((EffectInfo.GetValue(Caster.Level) / 100) + 1)
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                newSpeed /= ((EffectInfo.GetValue(Caster.Level) / 100) + 1)
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_MOUNTED_SPEED_ALWAYS(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                newSpeed *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).RunSpeed
                newSpeed /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.RUN, newSpeed)

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_SWIM_SPEED(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).SwimSpeed
                newSpeed *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).SwimSpeed = newSpeed
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.SWIM, newSpeed)

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).SwimSpeed
                newSpeed /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).SwimSpeed = newSpeed
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.SWIM, newSpeed)

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_FLY_SPEED(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).FlySpeed
                newSpeed *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).FlySpeed = newSpeed
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.FLY, newSpeed)
                CType(Target, CharacterObject).SetCanFly()

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).FlySpeed
                newSpeed /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).FlySpeed = newSpeed
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.FLY, newSpeed)
                CType(Target, CharacterObject).UnSetCanFly()

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_MOUNTED_FLY_SPEED(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).FlySpeed
                newSpeed *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).FlySpeed = newSpeed
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.FLY, newSpeed)
                CType(Target, CharacterObject).SetCanFly()

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).FlySpeed
                newSpeed /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).FlySpeed = newSpeed
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.FLY, newSpeed)
                CType(Target, CharacterObject).UnSetCanFly()

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_MOUNTED_FLY_SPEED_ALWAYS(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_ADD
                Dim newSpeed As Single = CType(Target, CharacterObject).FlySpeed
                newSpeed *= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).FlySpeed = newSpeed
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.FLY, newSpeed)
                CType(Target, CharacterObject).SetCanFly()

            Case AuraAction.AURA_REMOVE
                Dim newSpeed As Single = CType(Target, CharacterObject).FlySpeed
                newSpeed /= (EffectInfo.GetValue(Caster.Level) / 100) + 1
                CType(Target, CharacterObject).FlySpeed = newSpeed
                CType(Target, CharacterObject).ChangeSpeedForced(WS_CharManagment.CharacterObject.ChangeSpeedType.FLY, newSpeed)
                CType(Target, CharacterObject).UnSetCanFly()

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select
    End Sub

    Public Sub SPELL_AURA_MOUNTED(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Target.RemoveAurasOfType(AuraEffects_Names.SPELL_AURA_MOD_SHAPESHIFT)       'Remove shapeshift forms
                Target.RemoveAurasOfType(AuraEffects_Names.SPELL_AURA_MOUNTED, SpellID)     'Remove other mounted spells
                Target.RemoveAurasByInterruptFlag(SpellAuraInterruptFlags.AURA_INTERRUPT_FLAG_MOUNTING)

                If Not CREATURESDatabase.ContainsKey(EffectInfo.MiscValue) Then
                    Dim creature As New CreatureInfo(EffectInfo.MiscValue)
                End If
                If CREATURESDatabase.ContainsKey(EffectInfo.MiscValue) Then
                    Target.Mount = CType(CREATURESDatabase(EffectInfo.MiscValue), CreatureInfo).Model
                Else
                    Target.Mount = 0
                End If

            Case AuraAction.AURA_REMOVE
                Target.Mount = 0
                Target.RemoveAurasByInterruptFlag(SpellAuraInterruptFlags.AURA_INTERRUPT_FLAG_NOT_MOUNTED)

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select

        'DONE: Model update
        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MOUNTDISPLAYID, Target.Mount)
            CType(Target, CharacterObject).SendCharacterUpdate(True)
        Else
            Dim packet As New UpdatePacketClass
            Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
            tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_MOUNTDISPLAYID, Target.Mount)
            tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
            Target.SendToNearPlayers(CType(packet, UpdatePacketClass))
            tmpUpdate.Dispose()
            packet.Dispose()
        End If

    End Sub

    Public Sub SPELL_AURA_MOD_ROOT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Target.cUnitFlags = Target.cUnitFlags Or UnitFlags.UNIT_FLAG_ROOTED
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetMoveRoot()
                    CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_TARGET, CType(0, Long))
                End If

            Case AuraAction.AURA_REMOVE
                Target.cUnitFlags = Target.cUnitFlags And (Not UnitFlags.UNIT_FLAG_ROOTED)
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetMoveUnroot()
                End If

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select

        'DONE: Send update
        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_FLAGS, Target.cUnitFlags)
            CType(Target, CharacterObject).SendCharacterUpdate(True)
        Else
            Dim packet As New UpdatePacketClass
            Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
            tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_FLAGS, Target.cUnitFlags)
            tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
            Target.SendToNearPlayers(CType(packet, UpdatePacketClass))
            tmpUpdate.Dispose()
            packet.Dispose()
        End If

    End Sub
    Public Sub SPELL_AURA_MOD_STUN(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_ADD
                Target.cUnitFlags = Target.cUnitFlags Or UnitFlags.UNIT_FLAG_STUNTED
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetMoveRoot()
                    CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_TARGET, CType(0, Long))
                End If

            Case AuraAction.AURA_REMOVE
                Target.cUnitFlags = Target.cUnitFlags And (Not UnitFlags.UNIT_FLAG_STUNTED)
                If TypeOf Target Is CharacterObject Then
                    CType(Target, CharacterObject).SetMoveUnroot()
                End If

            Case AuraAction.AURA_UPDATE
                Exit Sub
        End Select

        'DONE: Send update
        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_FLAGS, Target.cUnitFlags)
            CType(Target, CharacterObject).SendCharacterUpdate(True)
        Else
            Dim packet As New UpdatePacketClass
            Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
            tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_FLAGS, Target.cUnitFlags)
            tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
            Target.SendToNearPlayers(CType(packet, UpdatePacketClass))
            tmpUpdate.Dispose()
            packet.Dispose()
        End If

    End Sub

    Public Sub SPELL_AURA_SAFE_FALL(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Dim packet As New PacketClass(OPCODES.SMSG_MOVE_FEATHER_FALL)
                packet.AddPackGUID(Target.GUID)
                Target.SendToNearPlayers(packet)
                packet.Dispose()

            Case AuraAction.AURA_REMOVE
                Dim packet As New PacketClass(OPCODES.SMSG_MOVE_NORMAL_FALL)
                packet.AddPackGUID(Target.GUID)
                Target.SendToNearPlayers(packet)
                packet.Dispose()

        End Select

    End Sub
    Public Sub SPELL_AURA_FEATHER_FALL(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Dim packet As New PacketClass(OPCODES.SMSG_MOVE_FEATHER_FALL)
                packet.AddPackGUID(Target.GUID)
                Target.SendToNearPlayers(packet)
                packet.Dispose()

            Case AuraAction.AURA_REMOVE
                Dim packet As New PacketClass(OPCODES.SMSG_MOVE_NORMAL_FALL)
                packet.AddPackGUID(Target.GUID)
                Target.SendToNearPlayers(packet)
                packet.Dispose()

        End Select

    End Sub
    Public Sub SPELL_AURA_WATER_WALK(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Dim packet As New PacketClass(OPCODES.SMSG_MOVE_SET_HOVER)
                packet.AddPackGUID(Target.GUID)
                Target.SendToNearPlayers(packet)
                packet.Dispose()

            Case AuraAction.AURA_REMOVE
                Dim packet As New PacketClass(OPCODES.SMSG_MOVE_UNSET_HOVER)
                packet.AddPackGUID(Target.GUID)
                Target.SendToNearPlayers(packet)
                packet.Dispose()

        End Select

    End Sub
    Public Sub SPELL_AURA_HOVER(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Dim packet As New PacketClass(OPCODES.SMSG_MOVE_WATER_WALK)
                packet.AddPackGUID(Target.GUID)
                Target.SendToNearPlayers(packet)
                packet.Dispose()

            Case AuraAction.AURA_REMOVE
                Dim packet As New PacketClass(OPCODES.SMSG_MOVE_LAND_WALK)
                packet.AddPackGUID(Target.GUID)
                Target.SendToNearPlayers(packet)
                packet.Dispose()

        End Select

    End Sub
    Public Sub SPELL_AURA_WATER_BREATHING(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                CType(Target, CharacterObject).underWaterBreathing = True

            Case AuraAction.AURA_REMOVE
                CType(Target, CharacterObject).underWaterBreathing = False

        End Select

    End Sub

    'TODO: Update values based on stats
    Public Sub SPELL_AURA_MOD_STAT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Action = AuraAction.AURA_UPDATE Then Exit Sub
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Dim value As Integer = EffectInfo.GetValue(Caster.Level)
        Dim value_sign As Integer = value
        If Action = AuraAction.AURA_REMOVE Then value = -value

        Select Case EffectInfo.MiscValue
            Case -1
                CType(Target, CharacterObject).Strength.Base /= CType(Target, CharacterObject).Strength.Modifier
                CType(Target, CharacterObject).Strength.Base += value
                CType(Target, CharacterObject).Strength.Base *= CType(Target, CharacterObject).Strength.Modifier
                CType(Target, CharacterObject).Agility.Base /= CType(Target, CharacterObject).Agility.Modifier
                CType(Target, CharacterObject).Agility.Base += value
                CType(Target, CharacterObject).Agility.Base *= CType(Target, CharacterObject).Agility.Modifier
                CType(Target, CharacterObject).Stamina.Base /= CType(Target, CharacterObject).Stamina.Modifier
                CType(Target, CharacterObject).Stamina.Base += value
                CType(Target, CharacterObject).Stamina.Base *= CType(Target, CharacterObject).Stamina.Modifier
                CType(Target, CharacterObject).Spirit.Base /= CType(Target, CharacterObject).Spirit.Modifier
                CType(Target, CharacterObject).Spirit.Base += value
                CType(Target, CharacterObject).Spirit.Base *= CType(Target, CharacterObject).Spirit.Modifier
                CType(Target, CharacterObject).Intellect.Base /= CType(Target, CharacterObject).Intellect.Modifier
                CType(Target, CharacterObject).Intellect.Base += value
                CType(Target, CharacterObject).Intellect.Base *= CType(Target, CharacterObject).Intellect.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Strength.PositiveBonus += value
                    CType(Target, CharacterObject).Agility.PositiveBonus += value
                    CType(Target, CharacterObject).Stamina.PositiveBonus += value
                    CType(Target, CharacterObject).Spirit.PositiveBonus += value
                    CType(Target, CharacterObject).Intellect.PositiveBonus += value
                Else
                    CType(Target, CharacterObject).Strength.NegativeBonus -= value
                    CType(Target, CharacterObject).Agility.NegativeBonus -= value
                    CType(Target, CharacterObject).Stamina.NegativeBonus -= value
                    CType(Target, CharacterObject).Spirit.NegativeBonus -= value
                    CType(Target, CharacterObject).Intellect.NegativeBonus -= value
                End If
            Case 0
                CType(Target, CharacterObject).Strength.Base /= CType(Target, CharacterObject).Strength.Modifier
                CType(Target, CharacterObject).Strength.Base += value
                CType(Target, CharacterObject).Strength.Base *= CType(Target, CharacterObject).Strength.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Strength.PositiveBonus += value
                Else
                    CType(Target, CharacterObject).Strength.NegativeBonus -= value
                End If
            Case 1
                CType(Target, CharacterObject).Agility.Base /= CType(Target, CharacterObject).Agility.Modifier
                CType(Target, CharacterObject).Agility.Base += value
                CType(Target, CharacterObject).Agility.Base *= CType(Target, CharacterObject).Agility.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Agility.PositiveBonus += value
                Else
                    CType(Target, CharacterObject).Agility.NegativeBonus -= value
                End If
            Case 2
                CType(Target, CharacterObject).Stamina.Base /= CType(Target, CharacterObject).Stamina.Modifier
                CType(Target, CharacterObject).Stamina.Base += value
                CType(Target, CharacterObject).Stamina.Base *= CType(Target, CharacterObject).Stamina.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Stamina.PositiveBonus += value
                Else
                    CType(Target, CharacterObject).Stamina.NegativeBonus -= value
                End If
            Case 3
                CType(Target, CharacterObject).Intellect.Base /= CType(Target, CharacterObject).Intellect.Modifier
                CType(Target, CharacterObject).Intellect.Base += value
                CType(Target, CharacterObject).Intellect.Base *= CType(Target, CharacterObject).Intellect.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Intellect.PositiveBonus += value
                Else
                    CType(Target, CharacterObject).Intellect.NegativeBonus -= value
                End If
            Case 4
                CType(Target, CharacterObject).Spirit.Base /= CType(Target, CharacterObject).Spirit.Modifier
                CType(Target, CharacterObject).Spirit.Base += value
                CType(Target, CharacterObject).Spirit.Base *= CType(Target, CharacterObject).Spirit.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Spirit.PositiveBonus += value
                Else
                    CType(Target, CharacterObject).Spirit.NegativeBonus -= value
                End If
        End Select

        CType(Target, CharacterObject).Life.Bonus = ((CType(Target, CharacterObject).Stamina.Base - 18) * 10)
        CType(Target, CharacterObject).Mana.Bonus = ((CType(Target, CharacterObject).Intellect.Base - 18) * 15)
        CType(Target, CharacterObject).Resistances(DamageTypes.DMG_PHYSICAL).Base += value * 2
        CType(Target, CharacterObject).UpdateManaRegen()

        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.BASE_ATTACK)
        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.OFF_ATTACK)
        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.RANGED_ATTACK)

        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_STRENGTH, CType(CType(Target, CharacterObject).Strength.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_AGILITY, CType(CType(Target, CharacterObject).Agility.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_STAMINA, CType(CType(Target, CharacterObject).Stamina.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_SPIRIT, CType(CType(Target, CharacterObject).Spirit.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_INTELLECT, CType(CType(Target, CharacterObject).Intellect.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT0, CType(CType(Target, CharacterObject).Strength.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT1, CType(CType(Target, CharacterObject).Agility.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT2, CType(CType(Target, CharacterObject).Stamina.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT3, CType(CType(Target, CharacterObject).Intellect.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT4, CType(CType(Target, CharacterObject).Spirit.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT0, CType(CType(Target, CharacterObject).Strength.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT1, CType(CType(Target, CharacterObject).Agility.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT2, CType(CType(Target, CharacterObject).Stamina.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT3, CType(CType(Target, CharacterObject).Intellect.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT4, CType(CType(Target, CharacterObject).Spirit.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_HEALTH, CType(Target, CharacterObject).Life.Current)
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXHEALTH, CType(Target, CharacterObject).Life.Maximum)
        If GetClassManaType(CType(Target, CharacterObject).Classe) = ManaTypes.TYPE_MANA Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1, CType(Target, CharacterObject).Mana.Current)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXPOWER1, CType(Target, CharacterObject).Mana.Maximum)
        End If
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + CByte(DamageTypes.DMG_PHYSICAL), CType(Target, CharacterObject).Resistances(DamageTypes.DMG_PHYSICAL).Base)
        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub
    Public Sub SPELL_AURA_MOD_STAT_PERCENT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Action = AuraAction.AURA_UPDATE Then Exit Sub
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        'TODO: This is only supposed to add % of the base stat, not the entire one.

        Dim value As Single = EffectInfo.GetValue(Caster.Level) / 100
        Dim value_sign As Integer = EffectInfo.GetValue(Caster.Level)
        If Action = AuraAction.AURA_REMOVE Then value = -value

        Dim OldStr As Short = CType(Target, CharacterObject).Strength.Base
        Dim OldAgi As Short = CType(Target, CharacterObject).Agility.Base
        Dim OldSta As Short = CType(Target, CharacterObject).Stamina.Base
        Dim OldSpi As Short = CType(Target, CharacterObject).Spirit.Base
        Dim OldInt As Short = CType(Target, CharacterObject).Intellect.Base

        Select Case EffectInfo.MiscValue
            Case -1
                CType(Target, CharacterObject).Strength.RealBase /= CType(Target, CharacterObject).Strength.BaseModifier
                CType(Target, CharacterObject).Strength.BaseModifier += value
                CType(Target, CharacterObject).Strength.RealBase *= CType(Target, CharacterObject).Strength.BaseModifier
                CType(Target, CharacterObject).Agility.RealBase /= CType(Target, CharacterObject).Agility.BaseModifier
                CType(Target, CharacterObject).Agility.BaseModifier += value
                CType(Target, CharacterObject).Agility.RealBase *= CType(Target, CharacterObject).Agility.BaseModifier
                CType(Target, CharacterObject).Stamina.RealBase /= CType(Target, CharacterObject).Stamina.BaseModifier
                CType(Target, CharacterObject).Stamina.BaseModifier += value
                CType(Target, CharacterObject).Stamina.RealBase *= CType(Target, CharacterObject).Stamina.BaseModifier
                CType(Target, CharacterObject).Spirit.RealBase /= CType(Target, CharacterObject).Spirit.BaseModifier
                CType(Target, CharacterObject).Spirit.BaseModifier += value
                CType(Target, CharacterObject).Spirit.RealBase *= CType(Target, CharacterObject).Spirit.BaseModifier
                CType(Target, CharacterObject).Intellect.RealBase /= CType(Target, CharacterObject).Intellect.BaseModifier
                CType(Target, CharacterObject).Intellect.BaseModifier += value
                CType(Target, CharacterObject).Intellect.RealBase *= CType(Target, CharacterObject).Intellect.BaseModifier
            Case 0
                CType(Target, CharacterObject).Strength.RealBase /= CType(Target, CharacterObject).Strength.BaseModifier
                CType(Target, CharacterObject).Strength.BaseModifier += value
                CType(Target, CharacterObject).Strength.RealBase *= CType(Target, CharacterObject).Strength.BaseModifier
            Case 1
                CType(Target, CharacterObject).Agility.RealBase /= CType(Target, CharacterObject).Agility.BaseModifier
                CType(Target, CharacterObject).Agility.BaseModifier += value
                CType(Target, CharacterObject).Agility.RealBase *= CType(Target, CharacterObject).Agility.BaseModifier
            Case 2
                CType(Target, CharacterObject).Stamina.RealBase /= CType(Target, CharacterObject).Stamina.BaseModifier
                CType(Target, CharacterObject).Stamina.BaseModifier += value
                CType(Target, CharacterObject).Stamina.RealBase *= CType(Target, CharacterObject).Stamina.BaseModifier
            Case 3
                CType(Target, CharacterObject).Intellect.RealBase /= CType(Target, CharacterObject).Intellect.BaseModifier
                CType(Target, CharacterObject).Intellect.BaseModifier += value
                CType(Target, CharacterObject).Intellect.RealBase *= CType(Target, CharacterObject).Intellect.BaseModifier
            Case 4
                CType(Target, CharacterObject).Spirit.RealBase /= CType(Target, CharacterObject).Spirit.BaseModifier
                CType(Target, CharacterObject).Spirit.BaseModifier += value
                CType(Target, CharacterObject).Spirit.RealBase *= CType(Target, CharacterObject).Spirit.BaseModifier
        End Select

        CType(Target, CharacterObject).Life.Bonus += (CType(Target, CharacterObject).Stamina.Base - OldSta) * 10
        CType(Target, CharacterObject).Mana.Bonus += (CType(Target, CharacterObject).Intellect.Base - OldInt) * 15
        CType(Target, CharacterObject).Resistances(DamageTypes.DMG_PHYSICAL).Base += (CType(Target, CharacterObject).Agility.Base - OldAgi) * 2
        CType(Target, CharacterObject).UpdateManaRegen()

        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.BASE_ATTACK)
        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.OFF_ATTACK)
        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.RANGED_ATTACK)

        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_STRENGTH, CType(CType(Target, CharacterObject).Strength.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_AGILITY, CType(CType(Target, CharacterObject).Agility.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_STAMINA, CType(CType(Target, CharacterObject).Stamina.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_SPIRIT, CType(CType(Target, CharacterObject).Spirit.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_INTELLECT, CType(CType(Target, CharacterObject).Intellect.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_HEALTH, CType(Target, CharacterObject).Life.Current)
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXHEALTH, CType(Target, CharacterObject).Life.Maximum)
        If GetClassManaType(CType(Target, CharacterObject).Classe) = ManaTypes.TYPE_MANA Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1, CType(Target, CharacterObject).Mana.Current)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXPOWER1, CType(Target, CharacterObject).Mana.Maximum)
        End If
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + CByte(DamageTypes.DMG_PHYSICAL), CType(Target, CharacterObject).Resistances(DamageTypes.DMG_PHYSICAL).Base)
        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub
    Public Sub SPELL_AURA_MOD_TOTAL_STAT_PERCENTAGE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Action = AuraAction.AURA_UPDATE Then Exit Sub
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Dim value As Single = EffectInfo.GetValue(Caster.Level) / 100
        Dim value_sign As Integer = EffectInfo.GetValue(Caster.Level)
        If Action = AuraAction.AURA_REMOVE Then value = -value

        Dim OldStr As Short = CType(Target, CharacterObject).Strength.Base
        Dim OldAgi As Short = CType(Target, CharacterObject).Agility.Base
        Dim OldSta As Short = CType(Target, CharacterObject).Stamina.Base
        Dim OldSpi As Short = CType(Target, CharacterObject).Spirit.Base
        Dim OldInt As Short = CType(Target, CharacterObject).Intellect.Base

        Select Case EffectInfo.MiscValue
            Case -1
                CType(Target, CharacterObject).Strength.Base /= CType(Target, CharacterObject).Strength.Modifier
                CType(Target, CharacterObject).Strength.Modifier += value
                CType(Target, CharacterObject).Strength.Base *= CType(Target, CharacterObject).Strength.Modifier
                CType(Target, CharacterObject).Agility.Base /= CType(Target, CharacterObject).Agility.Modifier
                CType(Target, CharacterObject).Agility.Modifier += value
                CType(Target, CharacterObject).Agility.Base *= CType(Target, CharacterObject).Agility.Modifier
                CType(Target, CharacterObject).Stamina.Base /= CType(Target, CharacterObject).Stamina.Modifier
                CType(Target, CharacterObject).Stamina.Modifier += value
                CType(Target, CharacterObject).Stamina.Base *= CType(Target, CharacterObject).Stamina.Modifier
                CType(Target, CharacterObject).Spirit.Base /= CType(Target, CharacterObject).Spirit.Modifier
                CType(Target, CharacterObject).Spirit.Modifier += value
                CType(Target, CharacterObject).Spirit.Base *= CType(Target, CharacterObject).Spirit.Modifier
                CType(Target, CharacterObject).Intellect.Base /= CType(Target, CharacterObject).Intellect.Modifier
                CType(Target, CharacterObject).Intellect.Modifier += value
                CType(Target, CharacterObject).Intellect.Base *= CType(Target, CharacterObject).Intellect.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Strength.PositiveBonus += (CType(Target, CharacterObject).Strength.Base - OldStr)
                    CType(Target, CharacterObject).Agility.PositiveBonus += (CType(Target, CharacterObject).Agility.Base - OldAgi)
                    CType(Target, CharacterObject).Stamina.PositiveBonus += (CType(Target, CharacterObject).Stamina.Base - OldSta)
                    CType(Target, CharacterObject).Spirit.PositiveBonus += (CType(Target, CharacterObject).Spirit.Base - OldSpi)
                    CType(Target, CharacterObject).Intellect.PositiveBonus += (CType(Target, CharacterObject).Intellect.Base - OldInt)
                Else
                    CType(Target, CharacterObject).Strength.NegativeBonus -= (CType(Target, CharacterObject).Strength.Base - OldStr)
                    CType(Target, CharacterObject).Agility.NegativeBonus -= (CType(Target, CharacterObject).Agility.Base - OldAgi)
                    CType(Target, CharacterObject).Stamina.NegativeBonus -= (CType(Target, CharacterObject).Stamina.Base - OldSta)
                    CType(Target, CharacterObject).Spirit.NegativeBonus -= (CType(Target, CharacterObject).Spirit.Base - OldSpi)
                    CType(Target, CharacterObject).Intellect.NegativeBonus -= (CType(Target, CharacterObject).Intellect.Base - OldInt)
                End If
            Case 0
                CType(Target, CharacterObject).Strength.Base /= CType(Target, CharacterObject).Strength.Modifier
                CType(Target, CharacterObject).Strength.Modifier += value
                CType(Target, CharacterObject).Strength.Base *= CType(Target, CharacterObject).Strength.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Strength.PositiveBonus += (CType(Target, CharacterObject).Strength.Base - OldStr)
                Else
                    CType(Target, CharacterObject).Strength.NegativeBonus -= (CType(Target, CharacterObject).Strength.Base - OldStr)
                End If
            Case 1
                CType(Target, CharacterObject).Agility.Base /= CType(Target, CharacterObject).Agility.Modifier
                CType(Target, CharacterObject).Agility.Modifier += value
                CType(Target, CharacterObject).Agility.Base *= CType(Target, CharacterObject).Agility.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Agility.PositiveBonus += (CType(Target, CharacterObject).Agility.Base - OldAgi)
                Else
                    CType(Target, CharacterObject).Agility.NegativeBonus -= (CType(Target, CharacterObject).Agility.Base - OldAgi)
                End If
            Case 2
                CType(Target, CharacterObject).Stamina.Base /= CType(Target, CharacterObject).Stamina.Modifier
                CType(Target, CharacterObject).Stamina.Modifier += value
                CType(Target, CharacterObject).Stamina.Base *= CType(Target, CharacterObject).Stamina.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Stamina.PositiveBonus += (CType(Target, CharacterObject).Stamina.Base - OldSta)
                Else
                    CType(Target, CharacterObject).Stamina.NegativeBonus -= (CType(Target, CharacterObject).Stamina.Base - OldSta)
                End If
            Case 3

            Case 4
                CType(Target, CharacterObject).Spirit.Base /= CType(Target, CharacterObject).Spirit.Modifier
                CType(Target, CharacterObject).Spirit.Modifier += value
                CType(Target, CharacterObject).Spirit.Base *= CType(Target, CharacterObject).Spirit.Modifier
                If value_sign > 0 Then
                    CType(Target, CharacterObject).Spirit.PositiveBonus += (CType(Target, CharacterObject).Spirit.Base - OldSpi)
                Else
                    CType(Target, CharacterObject).Spirit.NegativeBonus -= (CType(Target, CharacterObject).Spirit.Base - OldSpi)
                End If
        End Select

        CType(Target, CharacterObject).Life.Bonus = ((CType(Target, CharacterObject).Stamina.Base - 18) * 10)
        CType(Target, CharacterObject).Mana.Bonus = ((CType(Target, CharacterObject).Intellect.Base - 18) * 15)
        CType(Target, CharacterObject).Resistances(DamageTypes.DMG_PHYSICAL).Base += (CType(Target, CharacterObject).Agility.Base - OldAgi) * 2
        CType(Target, CharacterObject).UpdateManaRegen()

        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.BASE_ATTACK)
        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.OFF_ATTACK)
        CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.RANGED_ATTACK)

        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_STRENGTH, CType(CType(Target, CharacterObject).Strength.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_AGILITY, CType(CType(Target, CharacterObject).Agility.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_STAMINA, CType(CType(Target, CharacterObject).Stamina.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_SPIRIT, CType(CType(Target, CharacterObject).Spirit.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_INTELLECT, CType(CType(Target, CharacterObject).Intellect.Base, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT0, CType(CType(Target, CharacterObject).Strength.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT1, CType(CType(Target, CharacterObject).Agility.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT2, CType(CType(Target, CharacterObject).Stamina.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT3, CType(CType(Target, CharacterObject).Intellect.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POSSTAT4, CType(CType(Target, CharacterObject).Spirit.PositiveBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT0, CType(CType(Target, CharacterObject).Strength.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT1, CType(CType(Target, CharacterObject).Agility.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT2, CType(CType(Target, CharacterObject).Stamina.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT3, CType(CType(Target, CharacterObject).Intellect.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_NEGSTAT4, CType(CType(Target, CharacterObject).Spirit.NegativeBonus, Integer))
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_HEALTH, CType(Target, CharacterObject).Life.Current)
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXHEALTH, CType(Target, CharacterObject).Life.Maximum)
        If GetClassManaType(CType(Target, CharacterObject).Classe) = ManaTypes.TYPE_MANA Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_POWER1, CType(Target, CharacterObject).Mana.Current)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXPOWER1, CType(Target, CharacterObject).Mana.Maximum)
        End If
        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + CByte(DamageTypes.DMG_PHYSICAL), CType(Target, CharacterObject).Resistances(DamageTypes.DMG_PHYSICAL).Base)
        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub

    Public Sub SPELL_AURA_MOD_INCREASE_HEALTH(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.Life.Bonus += EffectInfo.GetValue(Caster.Level)

            Case AuraAction.AURA_REMOVE
                Target.Life.Bonus -= EffectInfo.GetValue(Caster.Level)

        End Select

        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXHEALTH, CType(Target.Life.Maximum, Integer))
        Else
            Dim packet As New UpdatePacketClass
            Dim UpdateData As New UpdateClass(EUnitFields.UNIT_END)
            UpdateData.SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXHEALTH, CType(Target.Life.Maximum, Integer))
            UpdateData.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject), 0)

            CType(Target, CreatureObject).SendToNearPlayers(CType(packet, UpdatePacketClass))
            packet.Dispose()
            UpdateData.Dispose()
        End If

    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_HEALTH_PERCENT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.Life.Modifier += (EffectInfo.GetValue(Target.Level) / 100)

            Case AuraAction.AURA_REMOVE
                Target.Life.Modifier -= (EffectInfo.GetValue(Target.Level) / 100)

        End Select

        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXHEALTH, CType(Target.Life.Maximum, Integer))
        Else
            Dim packet As New UpdatePacketClass
            Dim UpdateData As New UpdateClass(EUnitFields.UNIT_END)
            UpdateData.SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXHEALTH, CType(Target.Life.Maximum, Integer))
            UpdateData.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject), 0)

            CType(Target, CreatureObject).SendToNearPlayers(CType(packet, UpdatePacketClass))
            packet.Dispose()
            UpdateData.Dispose()
        End If

    End Sub
    Public Sub SPELL_AURA_MOD_INCREASE_ENERGY(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If EffectInfo.MiscValue = Target.ManaType Then
                    If Not TypeOf Target Is CharacterObject Then
                        Target.Mana.Bonus += EffectInfo.GetValue(Caster.Level)
                    Else
                        Select Case Target.ManaType
                            Case WS_CharManagment.ManaTypes.TYPE_ENERGY
                                CType(Target, CharacterObject).Energy.Bonus += EffectInfo.GetValue(Caster.Level)
                            Case WS_CharManagment.ManaTypes.TYPE_MANA
                                CType(Target, CharacterObject).Mana.Bonus += EffectInfo.GetValue(Caster.Level)
                            Case WS_CharManagment.ManaTypes.TYPE_RAGE
                                CType(Target, CharacterObject).Rage.Bonus += EffectInfo.GetValue(Caster.Level)
                        End Select
                    End If
                End If

            Case AuraAction.AURA_REMOVE
                If EffectInfo.MiscValue = Target.ManaType Then
                    If Not TypeOf Target Is CharacterObject Then
                        Target.Mana.Bonus -= EffectInfo.GetValue(Caster.Level)
                    Else
                        Select Case Target.ManaType
                            Case WS_CharManagment.ManaTypes.TYPE_ENERGY
                                CType(Target, CharacterObject).Energy.Bonus -= EffectInfo.GetValue(Caster.Level)
                            Case WS_CharManagment.ManaTypes.TYPE_MANA
                                CType(Target, CharacterObject).Mana.Bonus -= EffectInfo.GetValue(Caster.Level)
                            Case WS_CharManagment.ManaTypes.TYPE_RAGE
                                CType(Target, CharacterObject).Rage.Bonus -= EffectInfo.GetValue(Caster.Level)
                        End Select
                    End If
                End If
        End Select

        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXPOWER1 + ManaTypes.TYPE_ENERGY, CType(CType(Target, CharacterObject).Energy.Maximum, Integer))
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXPOWER1 + ManaTypes.TYPE_MANA, CType(CType(Target, CharacterObject).Mana.Maximum, Integer))
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXPOWER1 + ManaTypes.TYPE_RAGE, CType(CType(Target, CharacterObject).Rage.Maximum, Integer))
        Else
            Dim packet As New UpdatePacketClass
            Dim UpdateData As New UpdateClass(EUnitFields.UNIT_END)
            UpdateData.SetUpdateFlag(EUnitFields.UNIT_FIELD_MAXPOWER1 + Target.ManaType, CType(Target.Mana.Maximum, Integer))
            UpdateData.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject), 0)

            CType(Target, CreatureObject).SendToNearPlayers(CType(packet, UpdatePacketClass))
            packet.Dispose()
            UpdateData.Dispose()
        End If

    End Sub

    Public Sub SPELL_AURA_MOD_BASE_RESISTANCE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                For i As DamageTypes = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                        CType(Target, CharacterObject).Resistances(i).Base += EffectInfo.GetValue(Target.Level)
                        CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                    End If
                Next

            Case AuraAction.AURA_REMOVE
                For i As DamageTypes = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                        CType(Target, CharacterObject).Resistances(i).Base -= EffectInfo.GetValue(Target.Level)
                        CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                    End If
                Next

        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_BASE_RESISTANCE_PCT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                For i As Byte = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                        CType(Target, CharacterObject).Resistances(i).Modifier += (EffectInfo.GetValue(Target.Level) / 100)
                        CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                    End If
                Next

            Case AuraAction.AURA_REMOVE
                For i As Byte = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                        CType(Target, CharacterObject).Resistances(i).Modifier -= (EffectInfo.GetValue(Target.Level) / 100)
                        CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                    End If
                Next

        End Select
    End Sub
    Public Sub SPELL_AURA_MOD_RESISTANCE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                For i As Byte = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        If EffectInfo.GetValue(Target.Level) > 0 Then
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Base += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).PositiveBonus += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE + i, CType(Target, CharacterObject).Resistances(i).PositiveBonus)
                        Else
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Base += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).NegativeBonus -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE + i, CType(Target, CharacterObject).Resistances(i).NegativeBonus)
                        End If
                        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                    End If
                Next

            Case AuraAction.AURA_REMOVE
                For i As Byte = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        If EffectInfo.GetValue(Target.Level) > 0 Then
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Base -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).PositiveBonus -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE + i, CType(Target, CharacterObject).Resistances(i).PositiveBonus)
                        Else
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Base -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).NegativeBonus += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE + i, CType(Target, CharacterObject).Resistances(i).NegativeBonus)
                        End If
                        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                    End If
                Next

        End Select
        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub
    Public Sub SPELL_AURA_MOD_RESISTANCE_PCT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                For i As Byte = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        Dim OldBase As Short = CType(Target, CharacterObject).Resistances(i).Base
                        If EffectInfo.GetValue(Target.Level) > 0 Then
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Modifier += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).PositiveBonus += CType(Target, CharacterObject).Resistances(i).Base - OldBase
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE + i, CType(Target, CharacterObject).Resistances(i).PositiveBonus)
                        Else
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Modifier -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE + i, CType(Target, CharacterObject).Resistances(i).NegativeBonus)
                            CType(Target, CharacterObject).Resistances(i).PositiveBonus += CType(Target, CharacterObject).Resistances(i).Base - OldBase
                        End If
                        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                    End If
                Next

            Case AuraAction.AURA_REMOVE
                For i As Byte = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        Dim OldBase As Short = CType(Target, CharacterObject).Resistances(i).Base
                        If EffectInfo.GetValue(Target.Level) > 0 Then
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Modifier -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).PositiveBonus -= CType(Target, CharacterObject).Resistances(i).Base - OldBase
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE + i, CType(Target, CharacterObject).Resistances(i).PositiveBonus)
                        Else
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Modifier += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).NegativeBonus -= CType(Target, CharacterObject).Resistances(i).Base - OldBase
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE + i, CType(Target, CharacterObject).Resistances(i).NegativeBonus)
                        End If
                        CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                    End If
                Next

        End Select
        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub
    Public Sub SPELL_AURA_MOD_RESISTANCE_EXCLUSIVE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                For i As Byte = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        If EffectInfo.GetValue(Target.Level) > 0 Then
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Base += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).PositiveBonus += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE + i, CType(Target, CharacterObject).Resistances(i).PositiveBonus)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                        Else
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Base -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).NegativeBonus -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE + i, CType(Target, CharacterObject).Resistances(i).NegativeBonus)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                        End If
                    End If
                Next

            Case AuraAction.AURA_REMOVE
                For i As Byte = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        If EffectInfo.GetValue(Target.Level) > 0 Then
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Base -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).PositiveBonus -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE + i, CType(Target, CharacterObject).Resistances(i).PositiveBonus)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                        Else
                            CType(Target, CharacterObject).Resistances(i).Base /= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).Base += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).Resistances(i).Base *= CType(Target, CharacterObject).Resistances(i).Modifier
                            CType(Target, CharacterObject).Resistances(i).PositiveBonus += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE + i, CType(Target, CharacterObject).Resistances(i).NegativeBonus)
                            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RESISTANCES + i, CType(Target, CharacterObject).Resistances(i).Base)
                        End If
                    End If
                Next

        End Select
        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub
    Public Sub SPELL_AURA_MOD_ATTACK_POWER(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.AttackPowerMods += EffectInfo.GetValue(Caster.Level)

            Case AuraAction.AURA_REMOVE
                Target.AttackPowerMods -= EffectInfo.GetValue(Caster.Level)

        End Select

        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_ATTACK_POWER, CType(Target, CharacterObject).AttackPower)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_ATTACK_POWER_MODS, CType(Target, CharacterObject).AttackPowerMods)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RANGED_ATTACK_POWER, CType(Target, CharacterObject).AttackPowerRanged)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RANGED_ATTACK_POWER_MODS, CType(Target, CharacterObject).AttackPowerModsRanged)

            CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.BASE_ATTACK)
            CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.OFF_ATTACK)
        End If

    End Sub
    Public Sub SPELL_AURA_MOD_RANGED_ATTACK_POWER(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.AttackPowerModsRanged += EffectInfo.GetValue(Caster.Level)

            Case AuraAction.AURA_REMOVE
                Target.AttackPowerModsRanged -= EffectInfo.GetValue(Caster.Level)

        End Select

        If TypeOf Target Is CharacterObject Then
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_ATTACK_POWER, CType(Target, CharacterObject).AttackPower)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_ATTACK_POWER_MODS, CType(Target, CharacterObject).AttackPowerMods)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RANGED_ATTACK_POWER, CType(Target, CharacterObject).AttackPowerRanged)
            CType(Target, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_RANGED_ATTACK_POWER_MODS, CType(Target, CharacterObject).AttackPowerModsRanged)

            CalculateMinMaxDamage(CType(Target, CharacterObject), WeaponAttackType.RANGED_ATTACK)
        End If

    End Sub

    Public Sub SPELL_AURA_MOD_HEALING_DONE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Dim Value As Integer = EffectInfo.GetValue(Caster.Level)
        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                CType(Target, CharacterObject).healing.PositiveBonus += Value

            Case AuraAction.AURA_REMOVE
                CType(Target, CharacterObject).healing.PositiveBonus -= Value
        End Select

        CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FIELD_MOD_HEALING_DONE_POS, CType(Target, CharacterObject).healing.PositiveBonus)
        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub

    Public Sub SPELL_AURA_MOD_HEALING_DONE_PCT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Dim Value As Single = (EffectInfo.GetValue(Caster.Level) / 100)
        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                CType(Target, CharacterObject).healing.Modifier += Value

            Case AuraAction.AURA_REMOVE
                CType(Target, CharacterObject).healing.Modifier -= Value
        End Select

        CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FIELD_MOD_HEALING_DONE_POS, CType(Target, CharacterObject).healing.Value)
        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub

    Public Sub SPELL_AURA_MOD_DAMAGE_DONE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                For i As DamageTypes = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        If EffectInfo.GetValue(Target.Level) > 0 Then
                            CType(Target, CharacterObject).spellDamage(i).PositiveBonus += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FIELD_MOD_DAMAGE_DONE_POS + i, CType(Target, CharacterObject).spellDamage(i).PositiveBonus)
                        Else
                            CType(Target, CharacterObject).spellDamage(i).NegativeBonus -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FIELD_MOD_DAMAGE_DONE_NEG + i, CType(Target, CharacterObject).spellDamage(i).NegativeBonus)
                        End If
                    End If
                Next

            Case AuraAction.AURA_REMOVE
                For i As DamageTypes = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        If EffectInfo.GetValue(Target.Level) > 0 Then
                            CType(Target, CharacterObject).spellDamage(i).PositiveBonus -= EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FIELD_MOD_DAMAGE_DONE_POS + i, CType(Target, CharacterObject).spellDamage(i).PositiveBonus)
                        Else
                            CType(Target, CharacterObject).spellDamage(i).NegativeBonus += EffectInfo.GetValue(Target.Level)
                            CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FIELD_MOD_DAMAGE_DONE_NEG + i, CType(Target, CharacterObject).spellDamage(i).NegativeBonus)
                        End If
                    End If
                Next
        End Select

        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub

    Public Sub SPELL_AURA_MOD_DAMAGE_DONE_PCT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                For i As DamageTypes = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        CType(Target, CharacterObject).spellDamage(i).Modifier += (EffectInfo.GetValue(Target.Level) / 100)
                        CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FIELD_MOD_DAMAGE_DONE_PCT + i, CType(Target, CharacterObject).spellDamage(i).Modifier)
                    End If
                Next

            Case AuraAction.AURA_REMOVE
                For i As DamageTypes = DamageTypes.DMG_PHYSICAL To DamageTypes.DMG_ARCANE
                    If HaveFlag(EffectInfo.MiscValue, i) Then
                        CType(Target, CharacterObject).spellDamage(i).Modifier -= (EffectInfo.GetValue(Target.Level) / 100)
                        CType(Target, CharacterObject).SetUpdateFlag(EPlayerFields.PLAYER_FIELD_MOD_DAMAGE_DONE_PCT + i, CType(Target, CharacterObject).spellDamage(i).Modifier)
                    End If
                Next
        End Select

        CType(Target, CharacterObject).SendCharacterUpdate(False)
    End Sub

    Public Sub SPELL_AURA_EMPATHY(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If TypeOf Target Is CreatureObject AndAlso CType(Target, CreatureObject).CreatureInfo.CreatureType = UNIT_TYPE.BEAST Then
                    Dim packet As New UpdatePacketClass
                    Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
                    tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_DYNAMIC_FLAGS, Target.cDynamicFlags Or DynamicFlags.UNIT_DYNFLAG_SPECIALINFO)
                    tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
                    CType(Caster, CharacterObject).Client.Send(CType(packet, UpdatePacketClass))
                    tmpUpdate.Dispose()
                    packet.Dispose()
                End If

            Case AuraAction.AURA_REMOVE
                If TypeOf Target Is CreatureObject AndAlso CType(Target, CreatureObject).CreatureInfo.CreatureType = UNIT_TYPE.BEAST Then
                    Dim packet As New UpdatePacketClass
                    Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
                    tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_DYNAMIC_FLAGS, Target.cDynamicFlags)
                    tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
                    CType(Caster, CharacterObject).Client.Send(CType(packet, UpdatePacketClass))
                    tmpUpdate.Dispose()
                    packet.Dispose()
                End If
        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_SILENCE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.Spell_Silenced = True

            Case AuraAction.AURA_REMOVE
                Target.Spell_Silenced = False

        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_PACIFY(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.Spell_Pacifyed = True

            Case AuraAction.AURA_REMOVE
                Target.Spell_Pacifyed = False

        End Select

    End Sub

    Public Sub SPELL_AURA_MOD_LANGUAGE(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                CType(Target, CharacterObject).Spell_Language = EffectInfo.MiscValue

            Case AuraAction.AURA_REMOVE
                CType(Target, CharacterObject).Spell_Language = -1

        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_POSSESS(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CreatureObject Then Exit Sub
        If Not TypeOf Caster Is CharacterObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If Target.Level > EffectInfo.GetValue(Caster.Level) Then Exit Sub

                Dim packet As New UpdatePacketClass
                Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
                tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_CHARMEDBY, Caster.GUID)
                tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
                CType(Caster, CharacterObject).Client.Send(CType(packet, UpdatePacketClass))
                packet.Dispose()
                tmpUpdate.Dispose()

                CType(Target, CreatureObject).aiScript.Reset()
                'SendPetInitialize(Caster, Target.GUID)

            Case AuraAction.AURA_REMOVE
                Dim packet As New UpdatePacketClass
                Dim tmpUpdate As New UpdateClass(EUnitFields.UNIT_END)
                tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_CHARMEDBY, 0)
                tmpUpdate.AddToPacket(CType(packet, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Target, CreatureObject))
                CType(Caster, CharacterObject).Client.Send(CType(packet, UpdatePacketClass))
                packet.Dispose()
                tmpUpdate.Dispose()

                CType(Target, CreatureObject).aiScript.Reset()

        End Select

    End Sub

    Public Sub SPELL_AURA_MOD_THREAT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)

        'NOTE: EffectInfo.MiscValue => DamageType (not used for now, till new combat sytem)

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                Target.Spell_ThreatModifier *= EffectInfo.GetValue(Caster.Level)

            Case AuraAction.AURA_REMOVE
                Target.Spell_ThreatModifier /= EffectInfo.GetValue(Caster.Level)

        End Select

    End Sub
    Public Sub SPELL_AURA_MOD_TOTAL_THREAT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        Dim Value As Integer

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                If TypeOf Target Is CharacterObject Then
                    Value = EffectInfo.GetValue(Target.Level)
                Else
                    Value = EffectInfo.GetValue(Caster.Level)
                End If

            Case AuraAction.AURA_REMOVE
                If TypeOf Target Is CharacterObject Then
                    Value = -EffectInfo.GetValue(Target.Level)
                Else
                    Value = -EffectInfo.GetValue(Caster.Level)
                End If
        End Select

        If TypeOf Target Is CharacterObject Then
            For Each CreatureGUID As ULong In CType(Target, CharacterObject).creaturesNear
                If Not CType(WORLD_CREATUREs(CreatureGUID), CreatureObject).aiScript Is Nothing AndAlso _
                CType(WORLD_CREATUREs(CreatureGUID), CreatureObject).aiScript.aiHateTable.ContainsKey(Target) Then
                    CType(WORLD_CREATUREs(CreatureGUID), CreatureObject).aiScript.OnGenerateHate(Target, Value)
                End If
            Next
        Else
            If Not CType(Target, CreatureObject).aiScript Is Nothing AndAlso _
            CType(Target, CreatureObject).aiScript.aiHateTable.ContainsKey(Caster) Then
                CType(Target, CreatureObject).aiScript.OnGenerateHate(Caster, Value)
            End If
        End If
    End Sub
    Public Sub SPELL_AURA_MOD_TAUNT(ByRef Target As BaseUnit, ByRef Caster As BaseUnit, ByRef EffectInfo As SpellEffect, ByVal SpellID As Integer, ByVal StackCount As Integer, ByVal Action As AuraAction)
        If Not TypeOf Target Is CreatureObject Then Exit Sub

        Select Case Action
            Case AuraAction.AURA_UPDATE
                Exit Sub

            Case AuraAction.AURA_ADD
                CType(Target, CreatureObject).aiScript.OnGenerateHate(Caster, 9999999)

            Case AuraAction.AURA_REMOVE
                CType(Target, CreatureObject).aiScript.OnGenerateHate(Caster, -9999999)

        End Select

    End Sub

#End Region

#Region "WS.Spells.Handlers.Duel"

    Const DUEL_COUNTDOWN As Integer = 3000              'in miliseconds
    Const DUEL_OUTOFBOUNDS_DISTANCE As Single = 20

    Public Const DUEL_COUNTER_START As Byte = 10
    Public Const DUEL_COUNTER_DISABLED As Byte = 11

    Public Sub CheckDuelDistance(ByRef c As CharacterObject)
        If GetDistance(CType(c, CharacterObject), WORLD_GAMEOBJECTs(c.DuelArbiter)) > DUEL_OUTOFBOUNDS_DISTANCE Then
            If c.DuelOutOfBounds = DUEL_COUNTER_DISABLED Then
                'DONE: Notify for out of bounds of the duel flag and start counting
                Dim packet As New PacketClass(OPCODES.SMSG_DUEL_OUTOFBOUNDS)
                c.Client.Send(packet)
                packet.Dispose()

                c.DuelOutOfBounds = DUEL_COUNTER_START
            End If
        Else
            If c.DuelOutOfBounds <> DUEL_COUNTER_DISABLED Then
                c.DuelOutOfBounds = DUEL_COUNTER_DISABLED

                'DONE: Notify for in bounds of the duel flag
                Dim packet As New PacketClass(OPCODES.SMSG_DUEL_INBOUNDS)
                c.Client.Send(packet)
                packet.Dispose()
            End If
        End If
    End Sub
    Public Sub DuelComplete(ByRef Winner As CharacterObject, ByRef Loser As CharacterObject)
        If Winner Is Nothing Then Exit Sub
        If Loser Is Nothing Then Exit Sub

        'DONE: First stop the fight
        Dim response As New PacketClass(OPCODES.SMSG_DUEL_COMPLETE)
        response.AddInt8(1)
        Winner.Client.SendMultiplyPackets(response)
        Loser.Client.SendMultiplyPackets(response)
        response.Dispose()

        'DONE: Stop timed attacks for both
        Winner.attackState.AttackStop()
        Loser.attackState.AttackStop()

        'DONE: Clear duel things
        CType(WORLD_GAMEOBJECTs(Winner.DuelArbiter), GameObjectObject).Despawn()

        Winner.DuelOutOfBounds = DUEL_COUNTER_DISABLED
        Winner.DuelArbiter = 0
        Winner.SetUpdateFlag(EPlayerFields.PLAYER_DUEL_ARBITER, 0)
        Winner.SetUpdateFlag(EPlayerFields.PLAYER_DUEL_TEAM, 0)

        Loser.DuelOutOfBounds = DUEL_COUNTER_DISABLED
        Loser.DuelArbiter = 0
        Loser.SetUpdateFlag(EPlayerFields.PLAYER_DUEL_ARBITER, 0)
        Loser.SetUpdateFlag(EPlayerFields.PLAYER_DUEL_TEAM, 0)

        'DONE: Update life
        If Loser.Life.Current = 0 Then
            Loser.Life.Current = 1
            Loser.SetUpdateFlag(EUnitFields.UNIT_FIELD_HEALTH, 1)
            Loser.SetUpdateFlag(EUnitFields.UNIT_NPC_EMOTESTATE, EmoteStates.ANIM_EMOTE_BEG)
        End If
        Loser.SendCharacterUpdate(True)
        Winner.SendCharacterUpdate(True)

        'DONE: Notify client
        Dim packet As New PacketClass(OPCODES.SMSG_DUEL_WINNER)
        packet.AddInt8(0)
        packet.AddString(Winner.Name)
        packet.AddInt8(1)
        packet.AddString(Loser.Name)
        Winner.Client.SendMultiplyPackets(packet)
        Winner.SendToNearPlayers(packet)
        packet.Dispose()

        'DONE: Beg emote for loser
        Dim SMSG_EMOTE As New PacketClass(OPCODES.SMSG_EMOTE)
        SMSG_EMOTE.AddInt32(Emotes.ONESHOT_BEG)
        SMSG_EMOTE.AddUInt64(Loser.GUID)
        Loser.Client.SendMultiplyPackets(SMSG_EMOTE)
        Loser.SendToNearPlayers(SMSG_EMOTE)
        SMSG_EMOTE.Dispose()

        'DONE: Final clearing (if we clear it before we can't get names)
        Dim tmpCharacter As CharacterObject
        tmpCharacter = Winner
        Loser.DuelPartner = Nothing
        tmpCharacter.DuelPartner = Nothing
        tmpCharacter = Nothing
    End Sub

    Public Sub On_CMSG_DUEL_ACCEPTED(ByRef packet As PacketClass, ByRef Client As ClientClass)
        If (packet.Data.Length - 1) < 13 Then Exit Sub
        packet.GetInt16()
        Dim GUID As ULong = packet.GetUInt64

        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_DUEL_ACCEPTED [{2:X}]", Client.IP, Client.Port, GUID)

        'NOTE: Only invited player must have GUID set up
        If Client.Character.DuelArbiter <> GUID Then Exit Sub

        Dim c1 As CharacterObject = Client.Character.DuelPartner
        Dim c2 As CharacterObject = Client.Character
        c1.DuelArbiter = GUID
        c2.DuelArbiter = GUID

        'DONE: Do updates
        c1.SetUpdateFlag(EPlayerFields.PLAYER_DUEL_ARBITER, c1.DuelArbiter)
        c1.SetUpdateFlag(EPlayerFields.PLAYER_DUEL_TEAM, 1)
        c2.SetUpdateFlag(EPlayerFields.PLAYER_DUEL_ARBITER, c2.DuelArbiter)
        c2.SetUpdateFlag(EPlayerFields.PLAYER_DUEL_TEAM, 2)
        c2.SendCharacterUpdate(True)
        c1.SendCharacterUpdate(True)

        'DONE: Start the duel
        Dim response As New PacketClass(OPCODES.SMSG_DUEL_COUNTDOWN)
        response.AddInt32(DUEL_COUNTDOWN)
        c1.Client.SendMultiplyPackets(response)
        c2.Client.SendMultiplyPackets(response)
        response.Dispose()
    End Sub
    Public Sub On_CMSG_DUEL_CANCELLED(ByRef packet As PacketClass, ByRef Client As ClientClass)
        If (packet.Data.Length - 1) < 13 Then Exit Sub
        packet.GetInt16()
        Dim GUID As ULong = packet.GetUInt64

        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_DUEL_CANCELLED [{2:X}]", Client.IP, Client.Port, GUID)

        'DONE: Clear for client
        CType(WORLD_GAMEOBJECTs(Client.Character.DuelArbiter), GameObjectObject).Despawn()
        Client.Character.DuelArbiter = 0
        Client.Character.DuelPartner.DuelArbiter = 0

        Dim response As New PacketClass(OPCODES.SMSG_DUEL_COMPLETE)
        response.AddInt8(0)
        Client.Character.Client.SendMultiplyPackets(response)
        Client.Character.DuelPartner.Client.SendMultiplyPackets(response)
        response.Dispose()

        'DONE: Final clearing
        Client.Character.DuelPartner.DuelPartner = Nothing
        Client.Character.DuelPartner = Nothing
    End Sub

#End Region
#Region "WS.Spells.Handlers"
    Public Sub On_CMSG_CAST_SPELL(ByRef packet As PacketClass, ByRef Client As ClientClass)
        If (packet.Data.Length - 1) < 10 Then Exit Sub
        packet.GetInt16()
        Dim spellID As Integer = packet.GetInt32
        Dim Extra_Cast_Number As Byte = packet.GetInt8
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_CAST_SPELL [spellID={2}]", Client.IP, Client.Port, spellID)
        If Not Client.Character.HaveSpell(spellID) Then
            Log.WriteLine(LogType.WARNING, "[{0}:{1}] CHEAT: Character {2} casting unlearned spell {3}!", Client.IP, Client.Port, Client.Character.Name, spellID)
            Exit Sub
        End If
        If SPELLs.ContainsKey(spellID) = False Then
            Log.WriteLine(LogType.WARNING, "[{0}:{1}] Character tried to cast a spell that didn't exist: {2}!", Client.IP, Client.Port, spellID)
            Exit Sub
        End If

        DumpPacket(packet.Data, Client)
        'TODO: In duel disable

        Dim Targets As New SpellTargets
        Dim castResult As SpellFailedReason = SpellFailedReason.CAST_FAIL_ERROR
        Try
            Targets.ReadTargets(packet, CType(Client.Character, CharacterObject))
            castResult = CType(SPELLs(spellID), SpellInfo).CanCast(Client.Character, Targets)
            If Client.Character.spellCastState <> SpellCastState.SPELL_STATE_IDLE Then castResult = SpellFailedReason.CAST_FAIL_SPELL_IN_PROGRESS
            If castResult = SpellFailedReason.CAST_NO_ERROR Then
                Dim tmpSpell As New CastSpellParameters
                tmpSpell.tmpTargets = Targets
                tmpSpell.tmpCaster = Client.Character
                tmpSpell.tmpSpellID = spellID
                tmpSpell.tmpCN = Extra_Cast_Number
                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf tmpSpell.Cast))
            Else
                SendCastResult(castResult, Client, spellID, Extra_Cast_Number)
            End If

        Catch e As Exception
            Log.WriteLine(LogType.FAILED, "Error casting spell {0}.{1}", spellID, vbNewLine & e.ToString)
            SendCastResult(castResult, Client, spellID, Extra_Cast_Number)
        End Try
    End Sub
    Public Sub On_CMSG_CANCEL_CAST(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_CANCEL_CAST", Client.IP, Client.Port)

        'TODO: Other players can't see when you are interrupting your spells
        If Client.Character.spellCasted = 0 OrElse SPELLs.ContainsKey(Client.Character.spellCasted) = False Then Exit Sub
        Client.Character.spellCastState = SpellCastState.SPELL_STATE_IDLE
        SPELLs(Client.Character.spellCasted).SendInterrupted(0, Client.Character)
        SendCastResult(SpellFailedReason.CAST_FAIL_INTERRUPTED, Client, Client.Character.spellCasted, 0)
        Client.Character.spellCasted = 0
    End Sub
    Public Sub On_CMSG_CANCEL_AUTO_REPEAT_SPELL(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_CANCEL_AUTO_REPEAT_SPELL", Client.IP, Client.Port)

        Client.Character.spellCastState = SpellCastState.SPELL_STATE_IDLE
    End Sub

    Public Sub On_CMSG_CANCEL_AURA(ByRef packet As PacketClass, ByRef Client As ClientClass)
        If (packet.Data.Length - 1) < 9 Then Exit Sub
        packet.GetInt16()
        Dim spellID As Integer = packet.GetInt32
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_CANCEL_AURA [spellID={2}]", Client.IP, Client.Port, spellID)

        Client.Character.RemoveAuraBySpell(spellID)
    End Sub

    Public Sub On_CMSG_LEARN_TALENT(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Try
            If (packet.Data.Length - 1) < 13 Then Exit Sub
            packet.GetInt16()
            Dim TalentID As Integer = packet.GetInt32()
            Dim RequestedRank As Integer = packet.GetInt32()
            Dim CurrentTalentPoints As Byte = Client.Character.TalentPoints
            Dim SpellID As Integer
            Dim ReSpellID As Integer
            Dim i As Integer, j As Integer
            Dim HasEnoughRank As Boolean
            Dim DependsOn As Integer
            Dim SpentPoints As Integer

            Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_LEARN_TALENT [{2}:{3}]", Client.IP, Client.Port, TalentID, RequestedRank)

            If CurrentTalentPoints = 0 Then Exit Sub
            If RequestedRank > 4 Then Exit Sub

            'DONE: Now the character can't cheat, he must have the earlier rank to get the new one
            If RequestedRank > 0 Then
                If Not Client.Character.HaveSpell(CType(Talents(TalentID), TalentInfo).RankID(RequestedRank - 1)) Then
                    Exit Sub
                End If
            End If

            'DONE: Now the character can't cheat, he must have the other talents that is needed to get this one
            For j = 0 To 2
                If CType(Talents(TalentID), TalentInfo).RequiredTalent(j) > 0 Then
                    HasEnoughRank = False
                    DependsOn = CType(Talents(TalentID), TalentInfo).RequiredTalent(j)
                    For i = CType(Talents(TalentID), TalentInfo).RequiredPoints(j) To 4
                        If CType(Talents(DependsOn), TalentInfo).RankID(i) <> 0 Then
                            If Client.Character.HaveSpell(CType(Talents(DependsOn), TalentInfo).RankID(i)) Then
                                HasEnoughRank = True
                            End If
                        End If
                    Next i

                    If HasEnoughRank = False Then Exit Sub
                End If
            Next j

            'DONE: Count spent talent points
            SpentPoints = 0
            If CType(Talents(TalentID), TalentInfo).Row > 0 Then
                For Each TalentInfo As KeyValuePair(Of Integer, TalentInfo) In Talents
                    If CType(Talents(TalentID), TalentInfo).TalentTab = CType(TalentInfo.Value, TalentInfo).TalentTab Then
                        For i = 0 To 4
                            If CType(TalentInfo.Value, TalentInfo).RankID(i) <> 0 Then
                                If Client.Character.HaveSpell(CType(TalentInfo.Value, TalentInfo).RankID(i)) Then
                                    SpentPoints += i + 1
                                End If
                            End If
                        Next i
                    End If
                Next
            End If

#If DEBUG Then
            Log.WriteLine(LogType.INFORMATION, "Talents spent: {0}", SpentPoints)
#End If

            If SpentPoints < (CType(Talents(TalentID), TalentInfo).Row * 5) Then Exit Sub

            SpellID = CType(Talents(TalentID), TalentInfo).RankID(RequestedRank)

            If SpellID = 0 Then Exit Sub

            If Client.Character.HaveSpell(SpellID) Then Exit Sub

            Client.Character.LearnSpell(SpellID)

            'DONE: Cast passive talents on the character
            If SPELLs.ContainsKey(SpellID) AndAlso (CType(SPELLs(SpellID), SpellInfo).IsPassive) Then Client.Character.ApplySpell(SpellID)

            'DONE: Unlearning the earlier rank of the talent
            If RequestedRank > 0 Then
                ReSpellID = CType(Talents(TalentID), TalentInfo).RankID(RequestedRank - 1)
                Client.Character.UnLearnSpell(ReSpellID)
                Client.Character.RemoveAuraBySpell(ReSpellID)
            End If

            'DONE: Remove 1 talentpoint from the character
            Client.Character.TalentPoints -= 1
            Client.Character.SetUpdateFlag(EPlayerFields.PLAYER_CHARACTER_POINTS1, CType(Client.Character.TalentPoints, Integer))
            Client.Character.SendCharacterUpdate(True)

            Client.Character.SaveCharacter()
        Catch e As Exception
            Log.WriteLine(LogType.FAILED, "Error learning talen: {0}{1}", vbNewLine, e.ToString)
        End Try
    End Sub

#End Region

#Region "WS.Spells.Loot"
    Public Sub SendLoot(ByVal Player As BaseUnit, ByVal GUID As ULong, ByVal LootingType As LootType)
        If GuidIsGameObject(GUID) Then
            Select Case CType(WORLD_GAMEOBJECTs(GUID), GameObjectObject).ObjectInfo.Type
                Case GameObjectType.GAMEOBJECT_TYPE_DOOR, GameObjectType.GAMEOBJECT_TYPE_BUTTON
                    Exit Sub
                Case GameObjectType.GAMEOBJECT_TYPE_QUESTGIVER
                    Exit Sub
                Case GameObjectType.GAMEOBJECT_TYPE_SPELL_FOCUS
                    Exit Sub
                Case GameObjectType.GAMEOBJECT_TYPE_GOOBER
                    Exit Sub
                Case GameObjectType.GAMEOBJECT_TYPE_CHEST
                    'TODO: Script events
                    'Note: Don't exit sub here! We need the loot if it's a chest :P
            End Select
        End If

        'DONE: Sending loot
        CType(WORLD_GAMEOBJECTs(GUID), GameObjectObject).LootObject(CType(Player, CharacterObject), LootingType)
    End Sub
#End Region
End Module
