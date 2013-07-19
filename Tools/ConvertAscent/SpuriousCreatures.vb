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
Public Class SpuriousCreatures
#Region "Constants"
    Public v_id As Integer = 0
    Public v_name As String = " "
    Public v_guild As String = " "
    Public v_model As Integer = 0
    Public v_size As Single = 0
    Public v_life As Integer = 0
    Public v_mana As Integer = 0
    Public v_manatype As Integer = 0
    Public v_elite As Integer = 0
    Public v_faction As Integer = 0
    Public v_family As Single = 0
    Public v_type As Single = 0
    Public v_mindamage As Single = 0
    Public v_maxdamage As Single = 0
    Public v_minrangeddamage As Single = 0
    Public v_maxrangeddamage As Single = 0
    Public v_attackpower As Integer = 0
    Public v_rangedattackpower As Integer = 0
    Public v_armor As Integer = 0
    Public v_resholy As Integer = 0
    Public v_resfire As Integer = 0
    Public v_resnature As Integer = 0
    Public v_resfrost As Integer = 0
    Public v_resshadow As Integer = 0
    Public v_resarcane As Integer = 0
    Public v_walkspeed As Single = 0
    Public v_runspeed As Single = 0
    Public v_baseattackspeed As Single = 0
    Public v_baserangedattackspeed As Integer = 0
    Public v_combatreach As Single = 0
    Public v_bondingradius As Single = 0
    Public v_npcflags As Integer = 0
    Public v_flags As Integer = 0
    Public v_minlevel As Integer = 0
    Public v_maxlevel As Integer = 0
    Public v_loot As Integer = 0
    Public v_lootskinning As Integer = 0
    Public v_sell As Integer = 0
    Public v_aiscript As String = ""
#End Region

#Region "Properties"
    Public Property ID() As Integer
        Get
            ID = v_id
        End Get
        Set(ByVal Value As Integer)
            v_id = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Name = v_name
        End Get
        Set(ByVal Value As String)
            v_name = Value
        End Set
    End Property

    Public Property Guild() As String
        Get
            Guild = v_guild
        End Get
        Set(ByVal Value As String)
            v_guild = Value
        End Set
    End Property

    Public Property Model() As Integer
        Get
            Model = v_model
        End Get
        Set(ByVal Value As Integer)
            v_model = Value
        End Set
    End Property

    Public Property Size() As Single
        Get
            Size = v_size
        End Get
        Set(ByVal Value As Single)
            v_size = Value
        End Set
    End Property

    Public Property Life() As Integer
        Get
            Life = v_life
        End Get
        Set(ByVal Value As Integer)
            v_life = Value
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

    Public Property ManaType() As Integer
        Get
            ManaType = v_manatype
        End Get
        Set(ByVal Value As Integer)
            v_manatype = Value
        End Set
    End Property

    Public Property Elite() As Integer
        Get
            Elite = v_elite
        End Get
        Set(ByVal Value As Integer)
            v_elite = Value
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

    Public Property Family() As Single
        Get
            Family = v_family
        End Get
        Set(ByVal Value As Single)
            v_family = Value
        End Set
    End Property

    Public Property Type() As Single
        Get
            Type = v_type
        End Get
        Set(ByVal Value As Single)
            v_type = Value
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

    Public Property MinRangedDamage() As Single
        Get
            MinRangedDamage = v_minrangeddamage
        End Get
        Set(ByVal Value As Single)
            v_minrangeddamage = Value
        End Set
    End Property

    Public Property MaxRangedDamage() As Single
        Get
            MaxRangedDamage = v_maxrangeddamage
        End Get
        Set(ByVal Value As Single)
            v_maxrangeddamage = Value
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

    Public Property Armor() As Integer
        Get
            Armor = v_armor
        End Get
        Set(ByVal Value As Integer)
            v_armor = Value
        End Set
    End Property

    Public Property ResHoly() As Integer
        Get
            ResHoly = v_resholy
        End Get
        Set(ByVal Value As Integer)
            v_resholy = Value
        End Set
    End Property

    Public Property ResFire() As Integer
        Get
            ResFire = v_resfire
        End Get
        Set(ByVal Value As Integer)
            v_resfire = Value
        End Set
    End Property

    Public Property ResNature() As Integer
        Get
            ResNature = v_resnature
        End Get
        Set(ByVal Value As Integer)
            v_resnature = Value
        End Set
    End Property

    Public Property ResFrost() As Integer
        Get
            ResFrost = v_resfrost
        End Get
        Set(ByVal Value As Integer)
            v_resfrost = Value
        End Set
    End Property

    Public Property ResShadow() As Integer
        Get
            ResShadow = v_resshadow
        End Get
        Set(ByVal Value As Integer)
            v_resshadow = Value
        End Set
    End Property

    Public Property resArcane() As Integer
        Get
            resArcane = v_resarcane
        End Get
        Set(ByVal Value As Integer)
            v_resarcane = Value
        End Set
    End Property

    Public Property WalkSpeed() As Single
        Get
            WalkSpeed = v_walkspeed
        End Get
        Set(ByVal Value As Single)
            v_walkspeed = Value
        End Set
    End Property

    Public Property RunSpeed() As Single
        Get
            RunSpeed = v_runspeed
        End Get
        Set(ByVal Value As Single)
            v_runspeed = Value
        End Set
    End Property

    Public Property BaseAttackSpeed() As Single
        Get
            BaseAttackSpeed = v_baseattackspeed
        End Get
        Set(ByVal Value As Single)
            v_baseattackspeed = Value
        End Set
    End Property

    Public Property BaseRangedAttackSpeed() As Integer
        Get
            BaseRangedAttackSpeed = v_baserangedattackspeed
        End Get
        Set(ByVal Value As Integer)
            v_baserangedattackspeed = Value
        End Set
    End Property

    Public Property CombatReach() As Single
        Get
            CombatReach = v_combatreach
        End Get
        Set(ByVal Value As Single)
            v_combatreach = Value
        End Set
    End Property

    Public Property BondingRadius() As Single
        Get
            BondingRadius = v_bondingradius
        End Get
        Set(ByVal Value As Single)
            v_bondingradius = Value
        End Set
    End Property

    Public Property NpcFlags() As Integer
        Get
            NpcFlags = v_npcflags
        End Get
        Set(ByVal Value As Integer)
            v_npcflags = Value
        End Set
    End Property

    Public Property Flags() As Integer
        Get
            Flags = v_flags
        End Get
        Set(ByVal Value As Integer)
            v_flags = Value
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

    Public Property Loot() As Integer
        Get
            Loot = v_loot
        End Get
        Set(ByVal Value As Integer)
            v_loot = Value
        End Set
    End Property

    Public Property LootSkinning() As Integer
        Get
            LootSkinning = v_lootskinning
        End Get
        Set(ByVal Value As Integer)
            v_lootskinning = Value
        End Set
    End Property

    Public Property Sell() As Integer
        Get
            Sell = v_sell
        End Get
        Set(ByVal Value As Integer)
            v_sell = Value
        End Set
    End Property

    Public Property AIScript() As String
        Get
            AIScript = v_aiscript
        End Get
        Set(ByVal Value As String)
            v_aiscript = Value
        End Set
    End Property

#End Region

End Class
