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
Imports mangosVB.Common
Imports MySql.Data
Public Class Form1
    Public NCDB As New SQL
    Public NCDBdb As New SQL
    Public Spuriousdb As New SQL
    Public NCDBcreatures As New SQL
    Public Spuriouscreatures As New SQL
    Public NCDBspawns As New SQL
    Public Spuriousspawns As New SQL
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox("SectorSeven Tools" & vbCrLf & "Version 0.0.1" & vbCrLf & "Creative Commons License" & vbCrLf & "http://www.spuriuosemu.com")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Disable other buttons
        Button2.Enabled = False
        Button3.Enabled = False

        'declarations (Dims)
        Dim result As New DataTable 'there is a new Datatable to store
        Dim query As String 'says that query is a string
        Dim Creatures As New NCDBcreatures
        Dim NCDB As New SQL

        'converting creature_root
        query = "SELECT * FROM creature_root" 'gets NCDB creature table 1

        NCDB.Query(query, result) 'not sure, ask Trippy - may be storing queries from string query

        Me.lblStatus.Text = "Creatures conversion started..." 'updates the text of the status label
        Me.Refresh() 'Refreshes form 1

        Me.ProgressBar1.Minimum = 0 'not sure if this is correct :p, in ConvertAscent this is meant for
        'setting the minimum on the progress bar to 0. so that it starts at 0
        Me.ProgressBar1.Maximum = result.Rows.Count 'pretty sure this counts the number 
        'of rows in the datatable and then sets the number of rows as the maximum

        Dim i As Double = 0 'a variable that can store a double-precision floating point number
        '(whts that mean/used for?)
        For Each Row As DataRow In result.Rows 'result = a datatable, and you used a select query to 
            'select every entry in creatures, so for each row of an npc it does so and so
            i = i + 1 'basically says add 1 to i each time, kinda like a loop syntax
            Me.ProgressBar1.Value = i 'says that whatever number i is, is the value put in the progress
            'bar, so it will keep increasing because it is being added by 1 each time, correct?
            If (i \ 100) = Int(i \ 100) Then Me.Refresh() 'not really sure
            Creatures.name = CType(Row.Item("name").ToString, String) 'next line
            'ok so basically CType syntax is CType(expression, result)... the Row.Item to tostring
            'is the expression and the result is an integer apparently. 
            'http://msdn.microsoft.com/en-us/library/4x2877xb(VS.71).aspx
            Creatures.subname = CType(Row.Item("subname").ToString, String)
            Creatures.info_str = CType(Row.Item("info_str").ToString, Integer)
            Creatures.Flags1 = CType(Row.Item("Flags1").ToString, Integer)
            Creatures.type = CType(Row.Item("type").ToString, Integer)
            Creatures.family = CType(Row.Item("family").ToString, Integer)
            Creatures.rank = CType(Row.Item("rank").ToString, Integer)
            Creatures.spelldataid = CType(Row.Item("spelldataid").ToString, Integer)
            Creatures.displayid = CType(Row.Item("displayid").ToString, Integer)
            Creatures.displayid2 = CType(Row.Item("displayid2").ToString, Integer)
            Creatures.displayid3 = CType(Row.Item("displayid3").ToString, Integer)
            Creatures.displayid4 = CType(Row.Item("displayid4").ToString, Integer)
            Creatures.unknown_float1 = CType(Row.Item("unknown_float1").ToString, Single)
            Creatures.unknown_float2 = CType(Row.Item("unknown_float2").ToString, Single)
            If Row.Item("Leader").ToString <> "" Then
                Creatures.leader = CType(Row.Item("Leader").ToString, Integer)
            Else
                Creatures.leader = 0
            End If

            'Start of work on creature_template

            Dim result2 As New DataTable
            Dim query2 As String
            query2 = "SELECT * FROM creature_template entry='" & Trim(Creatures.Entry) & "';"
            NCDB.Query(query2, result2)                     'Since you not responding the above errror is because there isnt a Entry field in NCDBcreatures you either have to create on or change it to work with that datatable

            If result2.Rows.Count > 0 Then 'creature_template
                Creatures.MinLvl = CType(Row.Item("MinLvl").ToString, Integer)
                Creatures.MaxLvl = CType(Row.Item("MaxLvl").ToString, Integer)
                Creatures.Faction = CType(Row.Item("Faction").ToString, Integer)
                Creatures.MinHP = CType(Row.Item("MinHP").ToString, Integer)
                Creatures.MaxHP = CType(Row.Item("MaxHP").ToString, Integer)
                Creatures.Scale = CType(Row.Item("Scale").ToString, Integer)
                Creatures.NpcFlags = CType(Row.Item("NpcFlags").ToString, Integer)
                Creatures.AttackTime = CType(Row.Item("AttackTime").ToString, Integer)
                Creatures.AttackPower = CType(Row.Item("AttackPower").ToString, Integer)
                Creatures.RangedAttackPower = CType(Row.Item("Ranged Attack Power").ToString, Integer)
                Creatures.AttackType = CType(Row.Item("AttackType").ToString, Integer)
                Creatures.MinDmg = CType(Row.Item("MinDmg").ToString, Integer)
                Creatures.MaxDmg = CType(Row.Item("MaxDmg").ToString, Integer)
                Creatures.RangedAttackTime = CType(Row.Item("RangedAttackTime").ToString, Integer)
                Creatures.RangedMinDmg = CType(Row.Item("RangedMinDmg").ToString, Integer)
                Creatures.RangedMaxDmg = CType(Row.Item("RangedMaxDmg").ToString, Integer)
                Creatures.EquipmentEntry = CType(Row.Item("EquipmentEntry").ToString, Integer)
                Creatures.SpellGroup = CType(Row.Item("SpellGroup").ToString, Integer)
                Creatures.RespawnTime = CType(Row.Item("RespawnTime").ToString, Integer)
                Creatures.Armor = CType(Row.Item("Armor").ToString, Integer)
                Creatures.resistance1 = CType(Row.Item("resistance1").ToString, Integer)
                Creatures.resistance2 = CType(Row.Item("resistance2").ToString, Integer)
                Creatures.resistance3 = CType(Row.Item("resistance3").ToString, Integer)
                Creatures.resistance4 = CType(Row.Item("resistance4").ToString, Integer)
                Creatures.resistance5 = CType(Row.Item("resistance5").ToString, Integer)
                Creatures.resistance6 = CType(Row.Item("resistance6").ToString, Integer)
                Creatures.CombatReach = CType(Row.Item("CombatReach").ToString, Integer)
                Creatures.BoundingRadius = CType(Row.Item("BoundingRadius").ToString, Integer)
                Creatures.Auras = CType(Row.Item("Auras").ToString, Integer)
                Creatures.Boss = CType(Row.Item("Boss").ToString, Integer)
                Creatures.Money = CType(Row.Item("Money").ToString, Integer)
                Creatures.InvisibilityType = CType(Row.Item("InvisibilityType").ToString, Integer)
                Creatures.DeathState = CType(Row.Item("DeathState").ToString, Integer)
                Creatures.WalkSpeed = CType(Row.Item("WalkSpeed").ToString, Integer)
                Creatures.RunSpeed = CType(Row.Item("RunSpeed").ToString, Integer)
                Creatures.FlySpeed = CType(Row.Item("FlySpeed").ToString, Integer)
                Creatures.LootId = CType(Row.Item("LootId").ToString, Integer)
                Creatures.SkinLoot = CType(Row.Item("SkinLoot").ToString, Integer)
                Creatures.PickPocketLoot = CType(Row.Item("PickPocketLoot").ToString, Integer)
                Creatures.InhabitType = CType(Row.Item("InhabitType").ToString, Integer)
                Creatures.HasRegan = CType(Row.Item("HasRegan").ToString, Integer)
                Creatures.RespawnMod = CType(Row.Item("RespawnMod").ToString, Integer)
                Creatures.ArmorMod = CType(Row.Item("ArmorMod").ToString, Integer)
                Creatures.DamageMod = CType(Row.Item("DamageMod").ToString, Integer)
                Creatures.HealthMod = CType(Row.Item("HealthMod").ToString, Integer)
                Creatures.ExtraA9Flags = CType(Row.Item("ExtraA9Flags").ToString, Integer)
                Creatures.manatype = CType(Row.Item("manatype").ToString, Integer)
                Creatures.aiscript = CType(Row.Item("aiscript").ToString, Integer)
                Creatures.sell = CType(Row.Item("sell").ToString, Integer)
                Creatures.Resistance0_Armor = CType(Row.Item("Resistance0_Armor").ToString, Integer)
            End If
        Next
        Dim SpuriousDB As New SQL
        Creatures.manatype = 0
        Creatures.aiscript = 0
        Creatures.sell = 0
        Creatures.Resistance0_Armor = 0
        SpuriousDB.Update(String.Format("INSERT INTO creatures (creature_id, creature_name, creature_guild, creature_model, creature_size, creature_life, creature_mana, creature_manaType, creature_elite, creature_faction, creature_family, creature_type, creature_minDamage, creature_maxDamage, creature_minRangedDamage, creature_maxRangedDamage, creature_attackPower, creature_rangedAttackPower, creature_armor, creature_resHoly, creature_resFire, creature_resNature, creature_resFrost, creature_resShadow, creature_resArcane, creature_walkSpeed, creature_runSpeed, creature_baseAttackSpeed, creature_baseRangedAttackSpeed, creature_combatReach, creature_bondingRadius, creature_npcFlags, creature_flags, creature_minLevel, creature_maxLevel, creature_loot, creature_lootSkinning, creature_sell, creature_aiScript) VALUES ({0}, ""{1}"", ""{2}"", {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}, {35}, {36}, {37}, ""{38}"");", Creatures.Entry, Creatures.name, Creatures.subname, Creatures.displayid, Creatures.Scale, Creatures.MaxHP, Creatures.Mana, Creatures.manatype, Creatures.rank, Creatures.Faction, Creatures.family, Creatures.type, Creatures.MinDmg, Creatures.MaxDmg, Creatures.RangedMinDmg, Creatures.RangedMaxDmg, Creatures.AttackTime, Creatures.RangedAttackTime, Creatures.Resistance0_Armor, Creatures.resistance1, Creatures.resistance2, Creatures.resistance3, Creatures.resistance4, Creatures.resistance5, Creatures.resistance6, Creatures.WalkSpeed, Creatures.RunSpeed, Creatures.AttackTime, Creatures.RangedAttackTime, Creatures.CombatReach, Creatures.BoundingRadius, Creatures.NpcFlags, Creatures.Flags1, Creatures.MinLvl, Creatures.MaxLvl, Creatures.LootId, Creatures.SkinLoot, Creatures.sell, Creatures.aiscript))
        If CheckBox5.Checked = True Then

        End If
        If CheckBox7.Checked = True Then

            'ncdbLoot()
        End If
    End Sub
End Class
