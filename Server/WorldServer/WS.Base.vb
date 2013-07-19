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

Imports mangosVB.Common.BaseWriter
Imports System.Threading
Imports System.Collections.Generic

Public Module WS_Base
    Public Class BaseObject
        Public GUID As ULong = 0
        Public CellX As Byte = 0
        Public CellY As Byte = 0

        Public positionX As Single = 0
        Public positionY As Single = 0
        Public positionZ As Single = 0
        Public orientation As Single = 0
        Public instance As UInteger = 0
        Public MapID As UInteger = 0

        Public SpawnID As Integer = 0
        Public SeenBy As New List(Of ULong)

        Public Invisibility As InvisibilityLevel = InvisibilityLevel.VISIBLE
        Public Invisibility_Value As Integer = 0
        Public CanSeeInvisibility As InvisibilityLevel = InvisibilityLevel.INIVISIBILITY
        Public CanSeeInvisibility_Stealth As Integer = 0
        Public CanSeeInvisibility_Invisibility As Integer = 0
        Public Overridable Function CanSee(ByRef c As BaseObject) As Boolean
            If GUID = c.GUID Then Return False
            If instance <> c.instance Then Return False

            'DONE: GM and DEAD invisibility
            If c.Invisibility > CanSeeInvisibility Then Return False
            'DONE: Stealth Detection
            If c.Invisibility = InvisibilityLevel.STEALTH AndAlso (Math.Sqrt((c.positionX - positionX) ^ 2 + (c.positionY - positionY) ^ 2) < DEFAULT_DISTANCE_DETECTION) Then Return True
            'DONE: Check invisibility
            If c.Invisibility = InvisibilityLevel.INIVISIBILITY AndAlso c.Invisibility_Value > CanSeeInvisibility_Invisibility Then Return False
            If c.Invisibility = InvisibilityLevel.STEALTH AndAlso c.Invisibility_Value > CanSeeInvisibility_Stealth Then Return False

            'DONE: Check distance
            If Math.Sqrt((c.positionX - positionX) ^ 2 + (c.positionY - positionY) ^ 2) > DEFAULT_DISTANCE_VISIBLE Then Return False
            Return True
        End Function
        Public Sub InvisibilityReset()
            Invisibility = InvisibilityLevel.VISIBLE
            Invisibility_Value = 0
            CanSeeInvisibility = InvisibilityLevel.INIVISIBILITY
            CanSeeInvisibility_Stealth = 0
            CanSeeInvisibility_Invisibility = 0
        End Sub

        Public Sub SendToNearPlayers(ByRef packet As PacketClass)
            For Each c As ULong In SeenBy.ToArray
                If CHARACTERs.ContainsKey(c) AndAlso CHARACTERs(c).Client IsNot Nothing Then CHARACTERs(c).Client.SendMultiplyPackets(packet)
            Next
        End Sub
    End Class

    Public Class BaseUnit
        Inherits BaseObject

        Public Const CombatReach_Base As Single = 2.0F

        Public cUnitFlags As Integer = UnitFlags.UNIT_FLAG_ATTACKABLE
        Public cDynamicFlags As Integer = 0 'DynamicFlags.UNIT_DYNFLAG_SPECIALINFO
        Public cBytes1 As Integer = 0
        Public cBytes2 As Integer = 0

        Public Level As Byte = 0
        Public Model As Integer = 0
        Public Mount As Integer = 0
        Public ManaType As ManaTypes = WS_CharManagment.ManaTypes.TYPE_MANA
        Public Life As New TStatBar(1, 1, 0)
        Public Mana As New TStatBar(1, 1, 0)
        Public Size As Single = 1.0

        Public SummonedBy As ULong = 0
        Public CreatedBy As ULong = 0
        Public CreatedBySpell As Integer = 0

        'Temporaly variables
        Public Spell_Silenced As Boolean = False
        Public Spell_Pacifyed As Boolean = False
        Public Spell_ThreatModifier As Single = 1.0F
        Public AttackPowerMods As Integer = 0
        Public AttackPowerModsRanged As Integer = 0
        Public dynamicObjects As New List(Of DynamicObjectObject)

        Public Overridable Sub Die(ByRef Attacker As BaseUnit)
            Log.WriteLine(LogType.WARNING, "BaseUnit can't die.")
        End Sub
        Public Overridable Sub DealDamage(ByVal Damage As Integer, Optional ByRef Attacker As BaseUnit = Nothing)
            Log.WriteLine(LogType.WARNING, "No damage dealt.")
        End Sub
        Public Overridable Sub DealDamageMagical(ByRef Damage As Integer, ByVal DamageType As DamageTypes, Optional ByRef Attacker As BaseUnit = Nothing)
            Log.WriteLine(LogType.WARNING, "No damage dealt.")
        End Sub
        Public Overridable Sub Heal(ByVal Damage As Integer, Optional ByRef Attacker As BaseUnit = Nothing)
            Log.WriteLine(LogType.WARNING, "No healing done.")
        End Sub
        Public Overridable Sub Energize(ByVal Damage As Integer, ByVal Power As ManaTypes, Optional ByRef Attacker As BaseUnit = Nothing)
            Log.WriteLine(LogType.WARNING, "No mana increase done.")
        End Sub
        Public Overridable ReadOnly Property isDead() As Boolean
            Get
                Return (Not (Life.Current > 0))
            End Get
        End Property

        'Spell Aura Managment
        Public ActiveSpells(MAX_AURA_EFFECTs - 1) As BaseActiveSpell
        Public ActiveSpells_Flags(MAX_AURA_EFFECT_FLAGs - 1) As Integer
        Public ActiveSpells_Count(MAX_AURA_EFFECT_FLAGs - 1) As Integer
        Public ActiveSpells_Level(MAX_AURA_EFFECT_FLAGs - 1) As Integer
        Public Sub SetAura(ByVal SpellID As Integer, ByVal Slot As Integer, ByVal Duration As Integer)
            'DONE: Passive auras are not displayed
            If SpellID <> 0 AndAlso SPELLs.ContainsKey(SpellID) AndAlso CType(SPELLs(SpellID), SpellInfo).IsPassive Then Exit Sub

            'DONE: Calculating slots
            Dim AuraFlag_Slot As Integer = Slot \ 4
            Dim AuraFlag_SubSlot As Integer = (Slot Mod 4) * 8

            'DONE: Check if the spell is negative
            Dim Positive As Boolean = True
            If SpellID Then Positive = (Not CType(SPELLs(SpellID), SpellInfo).IsNegative)
            ActiveSpells_Flags(AuraFlag_Slot) = (ActiveSpells_Flags(AuraFlag_Slot) And (Not (&HFF << AuraFlag_SubSlot)))
            If SpellID <> 0 Then
                If Positive Then
                    ActiveSpells_Flags(AuraFlag_Slot) = (ActiveSpells_Flags(AuraFlag_Slot) Or (&H1F << AuraFlag_SubSlot))
                Else
                    ActiveSpells_Flags(AuraFlag_Slot) = (ActiveSpells_Flags(AuraFlag_Slot) Or (&H9 << AuraFlag_SubSlot))
                End If
            End If

            Dim tmpLevel As Byte = 0
            If SpellID Then tmpLevel = CType(SPELLs(SpellID), SpellInfo).spellLevel
            SetAuraStackCount(Slot, 0)
            SetAuraSlotLevel(Slot, tmpLevel)

            'DONE: Sending updates
            If TypeOf Me Is CharacterObject Then
                CType(Me, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_AURA + Slot, SpellID)
                CType(Me, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_AURAFLAGS + AuraFlag_Slot, ActiveSpells_Flags(AuraFlag_Slot))
                CType(Me, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_AURAAPPLICATIONS + AuraFlag_Slot, ActiveSpells_Count(AuraFlag_Slot))
                CType(Me, CharacterObject).SetUpdateFlag(EUnitFields.UNIT_FIELD_AURALEVELS + AuraFlag_Slot, ActiveSpells_Level(AuraFlag_Slot))
                CType(Me, CharacterObject).SendCharacterUpdate(True)

                Dim SMSG_UPDATE_AURA_DURATION As New PacketClass(OPCODES.SMSG_UPDATE_AURA_DURATION)
                SMSG_UPDATE_AURA_DURATION.AddInt8(Slot)
                SMSG_UPDATE_AURA_DURATION.AddInt32(Duration)
                CType(Me, CharacterObject).Client.Send(SMSG_UPDATE_AURA_DURATION)
                SMSG_UPDATE_AURA_DURATION.Dispose()
            Else
                Dim tmpUpdate As New UpdateClass
                Dim tmpPacket As New UpdatePacketClass
                tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_AURA + Slot, SpellID)
                tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_AURAFLAGS + AuraFlag_Slot, ActiveSpells_Flags(AuraFlag_Slot))
                tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_AURAAPPLICATIONS + AuraFlag_Slot, ActiveSpells_Count(AuraFlag_Slot))
                tmpUpdate.SetUpdateFlag(EUnitFields.UNIT_FIELD_AURALEVELS + AuraFlag_Slot, ActiveSpells_Level(AuraFlag_Slot))
                tmpUpdate.AddToPacket(CType(tmpPacket, UpdatePacketClass), ObjectUpdateType.UPDATETYPE_VALUES, CType(Me, CreatureObject), 0)
                SendToNearPlayers(CType(tmpPacket, UpdatePacketClass))
                tmpPacket.Dispose()
                tmpUpdate.Dispose()
            End If

        End Sub
        Public Sub SetAuraStackCount(ByVal Slot As Integer, ByVal Count As Byte)
            'NOTE: Stack count is Zero based -> 2 means "Stacked 3 times"

            Dim AuraFlag_Slot As Integer = Slot \ 4
            Dim AuraFlag_SubSlot As Integer = (Slot Mod 4) * 8

            ActiveSpells_Count(AuraFlag_Slot) = ActiveSpells_Count(AuraFlag_Slot) And (Not (&HFF << AuraFlag_SubSlot))
            ActiveSpells_Count(AuraFlag_Slot) = ActiveSpells_Count(AuraFlag_Slot) Or (CInt(Count) << AuraFlag_SubSlot)
        End Sub
        Public Sub SetAuraSlotLevel(ByVal Slot As Integer, ByVal Level As Integer)
            Dim AuraFlag_Slot As Integer = Slot \ 4
            Dim AuraFlag_SubSlot As Integer = (Slot Mod 4) * 8

            ActiveSpells_Level(AuraFlag_Slot) = ActiveSpells_Level(AuraFlag_Slot) And (Not (&HFF << AuraFlag_SubSlot))
            ActiveSpells_Level(AuraFlag_Slot) = ActiveSpells_Level(AuraFlag_Slot) Or (Level << AuraFlag_SubSlot)
        End Sub
        Public Function HaveAura(ByVal SpellID As Integer) As Boolean
            For i As Byte = 0 To MAX_AURA_EFFECTs - 1
                If Not ActiveSpells(i) Is Nothing AndAlso ActiveSpells(i).SpellID = SpellID Then Return True
            Next
            Return False
        End Function
        Public Function HaveVisibleAura(ByVal SpellID As Integer) As Boolean
            For i As Byte = 0 To MAX_AURA_EFFECTs_VISIBLE - 1
                If Not ActiveSpells(i) Is Nothing AndAlso ActiveSpells(i).SpellID = SpellID Then Return True
            Next
            Return False
        End Function
        Public Function HavePassiveAura(ByVal SpellID As Integer) As Boolean
            For i As Byte = MAX_AURA_EFFECTs_VISIBLE To MAX_AURA_EFFECTs - 1
                If Not ActiveSpells(i) Is Nothing AndAlso ActiveSpells(i).SpellID = SpellID Then Return True
            Next
            Return False
        End Function
        Public Function HaveImmuneMechanic(ByVal Mechanic As Integer) As Boolean
            For i As Byte = 0 To MAX_AURA_EFFECTs_VISIBLE - 1
                If Not ActiveSpells(i) Is Nothing Then
                    If (Not ActiveSpells(i).Aura1_Info Is Nothing) AndAlso ActiveSpells(i).Aura1_Info.ApplyAuraIndex = AuraEffects_Names.SPELL_AURA_MECHANIC_IMMUNITY Then
                        If ActiveSpells(i).Aura1_Info.MiscValue = Mechanic Then Return True
                    End If
                    If (Not ActiveSpells(i).Aura2_Info Is Nothing) AndAlso ActiveSpells(i).Aura2_Info.ApplyAuraIndex = AuraEffects_Names.SPELL_AURA_MECHANIC_IMMUNITY Then
                        If ActiveSpells(i).Aura2_Info.MiscValue = Mechanic Then Return True
                    End If
                    If (Not ActiveSpells(i).Aura3_Info Is Nothing) AndAlso ActiveSpells(i).Aura3_Info.ApplyAuraIndex = AuraEffects_Names.SPELL_AURA_MECHANIC_IMMUNITY Then
                        If ActiveSpells(i).Aura3_Info.MiscValue = Mechanic Then Return True
                    End If
                End If
            Next
            Return False
        End Function
        Public Sub RemoveAura(ByVal Slot As Integer, ByRef Caster As BaseUnit)
            'DONE: Removing SpellAura
            If Not ActiveSpells(Slot) Is Nothing Then
                If Not ActiveSpells(Slot).Aura1 Is Nothing Then ActiveSpells(Slot).Aura1.Invoke(Me, Caster, ActiveSpells(Slot).Aura1_Info, ActiveSpells(Slot).SpellID, ActiveSpells(Slot).StackCount + 1, AuraAction.AURA_REMOVE)
                If Not ActiveSpells(Slot).Aura2 Is Nothing Then ActiveSpells(Slot).Aura2.Invoke(Me, Caster, ActiveSpells(Slot).Aura2_Info, ActiveSpells(Slot).SpellID, ActiveSpells(Slot).StackCount + 1, AuraAction.AURA_REMOVE)
                If Not ActiveSpells(Slot).Aura3 Is Nothing Then ActiveSpells(Slot).Aura3.Invoke(Me, Caster, ActiveSpells(Slot).Aura3_Info, ActiveSpells(Slot).SpellID, ActiveSpells(Slot).StackCount + 1, AuraAction.AURA_REMOVE)
            End If
            ActiveSpells(Slot) = Nothing

            If Slot < MAX_AURA_EFFECTs_VISIBLE Then SetAura(0, Slot, 0)
        End Sub



        Public Sub RemoveAuraBySpell(ByVal SpellID As Integer)
            'DONE: Real aura removing
            For i As Integer = 0 To MAX_AURA_EFFECTs - 1
                If Not ActiveSpells(i) Is Nothing AndAlso ActiveSpells(i).SpellID = SpellID Then
                    RemoveAura(i, ActiveSpells(i).SpellCaster)

                    'DONE: Removing additional spell auras (Mind Vision)
                    If (TypeOf Me Is CharacterObject) AndAlso _
                        (CType(Me, CharacterObject).DuelArbiter <> 0) AndAlso (CType(Me, CharacterObject).DuelPartner Is Nothing) Then
                        WORLD_CREATUREs(CType(Me, CharacterObject).DuelArbiter).RemoveAuraBySpell(SpellID)
                        CType(Me, CharacterObject).DuelArbiter = 0
                    End If
                    Exit Sub
                End If
            Next
        End Sub
        Public Sub RemoveAurasOfType(ByVal AuraIndex As AuraEffects_Names, Optional ByRef NotSpellID As Integer = 0)
            'DONE: Removing SpellAuras of a certain type
            Dim i As Integer
            For i = 0 To MAX_AURA_EFFECTs_VISIBLE - 1
                If Not ActiveSpells(i) Is Nothing AndAlso ActiveSpells(i).SpellID <> NotSpellID Then
                    If (Not ActiveSpells(i).Aura1_Info Is Nothing) AndAlso ActiveSpells(i).Aura1_Info.ApplyAuraIndex = AuraIndex Then
                        RemoveAura(i, ActiveSpells(i).SpellCaster)
                        GoTo NextAura
                    End If
                    If (Not ActiveSpells(i).Aura2_Info Is Nothing) AndAlso ActiveSpells(i).Aura2_Info.ApplyAuraIndex = AuraIndex Then
                        RemoveAura(i, ActiveSpells(i).SpellCaster)
                        GoTo NextAura
                    End If
                    If (Not ActiveSpells(i).Aura3_Info Is Nothing) AndAlso ActiveSpells(i).Aura3_Info.ApplyAuraIndex = AuraIndex Then
                        RemoveAura(i, ActiveSpells(i).SpellCaster)
                        GoTo NextAura
                    End If
                End If
NextAura:
            Next
        End Sub
        Public Sub RemoveAurasByMechanic(ByVal Mechanic As Integer)
            'DONE: Removing SpellAuras of a certain mechanic
            Dim i As Integer
            For i = 0 To MAX_AURA_EFFECTs_VISIBLE - 1
                If Not ActiveSpells(i) Is Nothing AndAlso SPELLs(ActiveSpells(i).SpellID).Mechanic = Mechanic Then
                    RemoveAura(i, ActiveSpells(i).SpellCaster)
                End If
            Next
        End Sub
        Public Sub RemoveAurasByInterruptFlag(ByVal AuraInterruptFlag As Integer)
            'DONE: Removing SpellAuras with a certain interruptflag
            Dim i As Integer
            For i = 0 To MAX_AURA_EFFECTs_VISIBLE - 1
                If Not ActiveSpells(i) Is Nothing Then
                    If SPELLs.ContainsKey(ActiveSpells(i).SpellID) AndAlso (CType(SPELLs(ActiveSpells(i).SpellID), SpellInfo).auraInterruptFlags And AuraInterruptFlag) Then
                        If (CType(SPELLs(ActiveSpells(i).SpellID), SpellInfo).procFlags And SpellAuraProcFlags.AURA_PROC_REMOVEONUSE) = 0 Then
                            RemoveAura(i, ActiveSpells(i).SpellCaster)
                        End If
                    End If
                End If
            Next
        End Sub
        Public Sub AddAura(ByVal SpellID As Integer, ByVal Duration As Integer, ByRef Caster As BaseUnit)
            Dim AuraStart As Integer = 0
            Dim AuraEnd As Integer = MAX_POSITIVE_AURA_EFFECTs - 1
            If CType(SPELLs(SpellID), SpellInfo).IsPassive Then
                AuraStart = MAX_AURA_EFFECTs_VISIBLE
                AuraEnd = MAX_AURA_EFFECTs
            ElseIf CType(SPELLs(SpellID), SpellInfo).IsNegative Then
                AuraStart = MAX_POSITIVE_AURA_EFFECTs
                AuraEnd = MAX_AURA_EFFECTs_VISIBLE - 1
            End If

            For slot As Integer = AuraStart To AuraEnd
                If ActiveSpells(slot) Is Nothing Then
                    'DONE: Adding New SpellAura
                    ActiveSpells(slot) = New BaseActiveSpell(SpellID, Duration)
                    ActiveSpells(slot).SpellCaster = Caster

                    If slot < MAX_AURA_EFFECTs_VISIBLE Then SetAura(SpellID, slot, Duration)
                    Exit For
                End If
            Next
        End Sub

        Public Property StandState() As Byte
            Get
                Return (cBytes1 And &HFF)
            End Get
            Set(ByVal Value As Byte)
                cBytes1 = ((cBytes1 And &HFFFFFF00) Or Value)
            End Set
        End Property

        Public Property ShapeshiftForm() As ShapeshiftForm
            Get
                Return CByte((cBytes2 And &HFF000000) >> 24)
            End Get
            Set(ByVal form As ShapeshiftForm)
                cBytes1 = ((cBytes2 And &HFFFFFF) Or (CInt(form) << 24))
            End Set
        End Property

    End Class

    Public Class BaseActiveSpell
        Public SpellID As Integer = 0
        Public SpellDuration As Integer = 0
        Public SpellCaster As BaseUnit = Nothing

        Public StackCount As Integer = 0

        Public Aura1 As ApplyAuraHandler = Nothing
        Public Aura2 As ApplyAuraHandler = Nothing
        Public Aura3 As ApplyAuraHandler = Nothing
        Public Aura1_Info As SpellEffect = Nothing
        Public Aura2_Info As SpellEffect = Nothing
        Public Aura3_Info As SpellEffect = Nothing

        Public Sub New(ByVal ID As Integer, ByVal Duration As Integer)
            SpellID = ID
            SpellDuration = Duration
        End Sub
        Public ReadOnly Property GetSpellInfo() As SpellInfo
            Get
                Return SPELLs(SpellID)
            End Get
        End Property
    End Class
End Module
