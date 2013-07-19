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
Imports mangosVB.Common

Public Class frmMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents btnCnvCreatures As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnCnvObjects As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnCnvCreatures = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.lblCount = New System.Windows.Forms.Label
        Me.btnCnvObjects = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnCnvCreatures
        '
        Me.btnCnvCreatures.Location = New System.Drawing.Point(124, 208)
        Me.btnCnvCreatures.Name = "btnCnvCreatures"
        Me.btnCnvCreatures.Size = New System.Drawing.Size(93, 32)
        Me.btnCnvCreatures.TabIndex = 0
        Me.btnCnvCreatures.Text = "Creatures"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.White
        Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStatus.Location = New System.Drawing.Point(16, 88)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(544, 23)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(16, 128)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(544, 23)
        Me.ProgressBar1.TabIndex = 2
        '
        'lblCount
        '
        Me.lblCount.Location = New System.Drawing.Point(256, 168)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(64, 24)
        Me.lblCount.TabIndex = 3
        Me.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCnvObjects
        '
        Me.btnCnvObjects.Location = New System.Drawing.Point(241, 208)
        Me.btnCnvObjects.Name = "btnCnvObjects"
        Me.btnCnvObjects.Size = New System.Drawing.Size(93, 32)
        Me.btnCnvObjects.TabIndex = 4
        Me.btnCnvObjects.Text = "Objects"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(362, 208)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(93, 32)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Sells"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(576, 266)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnCnvObjects)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnCnvCreatures)
        Me.Name = "frmMain"
        Me.Text = "Ascent To Spurious Conversion"
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "DataAccess"
    Public AscentDB As New SQL
    Public AscentDB2 As New SQL
    Public SpuriousDB As New SQL
    Public SCreatureDB As New SQL
    Public AscentVendors As New SQL
    Public SpuriousSells As New SQL
    Public Vendors As New AscentVendors
    Public SSpawnDB As New SQL
    Public manatype As Integer = 0
    Public loot As Integer = 0
    Public lootSkinning As Integer = 0
    Public sell As Integer = 0
    Public aiScript As String = ""
    Public spawn_spawned As Integer = 0
    Public spawn_range As Integer = 0
    Public spawn_type As Integer = 0
    Public spawn_left As Integer = 0
    Public spawn_waypoints As Integer = 0
    Public respawntime As Integer = 0
    Public InsertString As String = " "
    Public ASpawn As New AscentSpawns
    Public SCreatures As New SpuriousCreatures
    Public SSpawns As New SpuriousSpawns
    Public Objects As New AscentObjects

    Public Sub SLQEventHandler(ByVal MessageID As SQL.EMessages, ByVal OutBuf As String)
        Select Case MessageID
            Case SQL.EMessages.ID_Error
                MsgBox(OutBuf, MsgBoxStyle.Critical, "Error")
            Case SQL.EMessages.ID_Message
                MsgBox(OutBuf, MsgBoxStyle.Information, "Message")
        End Select
    End Sub
#End Region

#Region "Creatures"
    Private Sub btnCnvCreatures_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCnvCreatures.Click
        lblStatus.ForeColor = Color.Black

        ' Set Up and Connect to Ascent Database (for Creature_Names)
        AscentDB.SQLTypeServer = SQL.DB_Type.MySQL
        AscentDB.SQLHost = "localhost"
        AscentDB.SQLPort = "3306"
        AscentDB.SQLUser = "root"
        AscentDB.SQLPass = "root"
        AscentDB.SQLDBName = "ncdb"
        AscentDB.Connect()

        AddHandler AscentDB.SQLMessage, AddressOf SLQEventHandler

        ' Set Up and Connect to Spurious Database (for vbwow_creatures)
        SpuriousDB.SQLTypeServer = SQL.DB_Type.MySQL
        SpuriousDB.SQLHost = "localhost"
        SpuriousDB.SQLPort = "3306"
        SpuriousDB.SQLUser = "root"
        SpuriousDB.SQLPass = "root"
        SpuriousDB.SQLDBName = "spurious"
        SpuriousDB.Connect()

        lblStatus.Text = "Loading Creatures From Ascent..."
        Me.Refresh()

        Dim result As New DataTable
        Dim query As String
        query = "SELECT * FROM creature_root ORDER BY entry;"
        AscentDB.Query(query, result)

        lblStatus.Text = "Conversion In Progress...."
        Me.Refresh()
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = result.Rows.Count
        Dim i As Double = 0
        For Each Row As DataRow In result.Rows
            Dim Creatures As New AscentCreatures
            i = i + 1
            ProgressBar1.Value = i
            If (i \ 100) = Int(i \ 100) Then Me.Refresh()
            Creatures.Entry = CType(Row.Item("entry"), Integer)
            Creatures.Creature_Name = Row.Item("name").ToString.Replace("'", "\'")
            Creatures.Subname = Row.Item("Subname").ToString.Replace("'", "\'")
            Creatures.InfoStr = Row.Item("info_str")
            Creatures.Flags1 = CType(Row.Item("Flags1"), Integer)
            Creatures.Type = CType(Row.Item("type"), Integer)
            Creatures.Family = CType(Row.Item("Family"), Integer)
            Creatures.Rank = CType(Row.Item("Rank"), Integer)
            Creatures.unk4 = CType(Row.Item("unk4"), Integer)
            Creatures.SpellDataID = CType(Row.Item("spelldataid"), Integer)
            Creatures.DisplayID = CType(Row.Item("displayid"), Integer)
            Creatures.Female_DisplayID = CType(Row.Item("displayid2"), Integer)
            Creatures.Male_DisplayID2 = CType(Row.Item("displayid3"), Integer)
            Creatures.Female_DisplayID2 = CType(Row.Item("displayid4"), Integer)
            Creatures.Unknown_Float1 = CType(Row.Item("unknown_float1"), Single)
            Creatures.Unknown_Float2 = CType(Row.Item("unknown_float2"), Single)
            Creatures.Leader = CType(Row.Item("leader"), Integer)

            Dim proto As New DataTable
            Dim pquery As String
            pquery = "SELECT * FROM creature_template WHERE entry=" & Creatures.Entry & " LIMIT 1;"
            AscentDB.Query(pquery, proto)

            If proto.Rows.Count > 0 Then
                Creatures.MinLevel = CType(proto.Rows(0).Item("minlvl"), Integer)
                Creatures.MaxLevel = CType(proto.Rows(0).Item("maxlvl"), Integer)
                Creatures.Faction = CType(proto.Rows(0).Item("faction"), Integer)
                Creatures.MinHealth = CType(proto.Rows(0).Item("minhp"), Integer)
                Creatures.MaxHealth = CType(proto.Rows(0).Item("maxhp"), Integer)
                Creatures.Mana = CType(proto.Rows(0).Item("mana"), Integer)
                Creatures.Scale = CType(proto.Rows(0).Item("scale"), Single)
                Creatures.npcFlags = CType(proto.Rows(0).Item("npcflags"), Integer)
                Creatures.AttackTime = CType(proto.Rows(0).Item("attacktime"), Integer)
                Creatures.AttackPower = CType(proto.Rows(0).Item("attackpower"), Integer)
                Creatures.RangedAttackPower = CType(proto.Rows(0).Item("rangedattackpower"), Integer)
                Creatures.AttackType = CType(proto.Rows(0).Item("attacktype"), Integer)
                Creatures.MinDamage = CType(proto.Rows(0).Item("mindmg"), Single)
                Creatures.MaxDamage = CType(proto.Rows(0).Item("maxdmg"), Single)
                Creatures.RangedAttackTime = CType(proto.Rows(0).Item("rangedattacktime"), Integer)
                Creatures.RangedMinDamage = CType(proto.Rows(0).Item("rangedmindmg"), Single)
                Creatures.RangedMaxDamage = CType(proto.Rows(0).Item("rangedmaxdmg"), Single)
                Creatures.EquipmentEntry = CType(proto.Rows(0).Item("equipmententry"), Integer)
                Creatures.RespawnTime = CType(proto.Rows(0).Item("respawntime"), Integer)
                Creatures.Resistance0_Armor = CType(proto.Rows(0).Item("armor"), Integer)
                Creatures.Resistance1 = CType(proto.Rows(0).Item("resistance1"), Integer)
                Creatures.Resistance2 = CType(proto.Rows(0).Item("resistance2"), Integer)
                Creatures.Resistance3 = CType(proto.Rows(0).Item("resistance3"), Integer)
                Creatures.Resistance4 = CType(proto.Rows(0).Item("resistance4"), Integer)
                Creatures.Resistance5 = CType(proto.Rows(0).Item("resistance5"), Integer)
                Creatures.Resistance6 = CType(proto.Rows(0).Item("resistance6"), Integer)
                Creatures.Combat_Reach = CType(proto.Rows(0).Item("combatreach"), Single)
                Creatures.Bounding_Radius = CType(proto.Rows(0).Item("boundingradius"), Single)
                Creatures.Auras = proto.Rows(0).Item("auras")
                Creatures.Boss = CType(proto.Rows(0).Item("boss"), Integer)
                Creatures.Money = CType(proto.Rows(0).Item("money"), Integer)
                Creatures.Invisibility_Type = CType(proto.Rows(0).Item("invisibilitytype"), Integer)
                Creatures.Death_State = CType(proto.Rows(0).Item("deathstate"), Integer)
                Creatures.DynamicFlags = CType(proto.Rows(0).Item("dynamicflags"), Integer)
                Creatures.FlagsExtra = CType(proto.Rows(0).Item("flags_extra"), Integer)
                Creatures.MoveType = CType(proto.Rows(0).Item("movetype"), Integer)
                Creatures.WalkSpeed = CType(proto.Rows(0).Item("walkspeed"), Single)
                Creatures.RunSpeed = CType(proto.Rows(0).Item("runspeed"), Single)
                Creatures.FlySpeed = CType(proto.Rows(0).Item("flyspeed"), Single)
                Creatures.LootID = CType(proto.Rows(0).Item("lootid"), Integer)
                Creatures.SkinLoot = CType(proto.Rows(0).Item("skinloot"), Integer)
                Creatures.PickPocketLoot = CType(proto.Rows(0).Item("pickpocketloot"), Integer)
                Creatures.InhabitType = CType(proto.Rows(0).Item("inhabittype"), Integer)
                Creatures.HasRegan = CType(proto.Rows(0).Item("hasregan"), Integer)
                Creatures.RespawnMod = CType(proto.Rows(0).Item("respawnmod"), Single)
                Creatures.ArmorMod = CType(proto.Rows(0).Item("armormod"), Single)
                Creatures.DamageMod = CType(proto.Rows(0).Item("damagemod"), Single)
                Creatures.HealthMod = CType(proto.Rows(0).Item("healthmod"), Single)
                Creatures.ExtraA9Flags = CType(proto.Rows(0).Item("extraa9flags"), Integer)
            End If
            proto.Clear()

            ' Insert Information Into Creatures Table On Spurious Database
            SpuriousDB.Update(String.Format("INSERT INTO creatures (creature_id, creature_name, creature_guild, info_str, creature_model, creature_femalemodel, creature_size, creature_life, creature_mana, creature_manaType, creature_elite, creature_faction, creature_family, creature_type, creature_minDamage, creature_maxDamage, creature_minRangedDamage, creature_maxRangedDamage, creature_attackPower, creature_rangedAttackPower, creature_armor, creature_resHoly, creature_resFire, creature_resNature, creature_resFrost, creature_resShadow, creature_resArcane, creature_walkSpeed, creature_runSpeed, creature_flySpeed, creature_respawnTime, creature_baseAttackSpeed, creature_baseRangedAttackSpeed, creature_combatReach, creature_bondingRadius, creature_npcFlags, creature_flags, creature_minLevel, creature_maxLevel, creature_loot, creature_skinloot, creature_pickpocketloot, creature_equipmententry, creature_aiScript) VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40}, {41}, {42}, '{43}');", Creatures.Entry, Creatures.Creature_Name, Creatures.Subname, Creatures.InfoStr, Creatures.DisplayID, Creatures.Female_DisplayID, FixSingle(Creatures.Scale), Creatures.MaxHealth, Creatures.Mana, manatype, Creatures.Rank, Creatures.Faction, Creatures.Family, Creatures.Type, FixSingle(Creatures.MinDamage), FixSingle(Creatures.MaxDamage), FixSingle(Creatures.RangedMinDamage), FixSingle(Creatures.RangedMaxDamage), Creatures.AttackPower, Creatures.RangedAttackPower, Creatures.Resistance0_Armor, Creatures.Resistance1, Creatures.Resistance2, Creatures.Resistance3, Creatures.Resistance4, Creatures.Resistance5, Creatures.Resistance6, FixSingle(Creatures.WalkSpeed), FixSingle(Creatures.RunSpeed), FixSingle(Creatures.FlySpeed), Creatures.RespawnTime, Creatures.AttackTime, Creatures.RangedAttackTime, FixSingle(Creatures.Combat_Reach), FixSingle(Creatures.Bounding_Radius), Creatures.npcFlags, Creatures.Flags1, Creatures.MinLevel, Creatures.MaxLevel, Creatures.LootID, Creatures.SkinLoot, Creatures.PickPocketLoot, Creatures.EquipmentEntry, aiScript))
        Next

        Dim equipment As New DataTable
        Dim equery As String
        equery = "SELECT * FROM creature_equip_template ORDER by entry;"
        AscentDB.Query(equery, equipment)

        Dim EquipEntry As UInteger
        Dim EquipModel1 As UInteger
        Dim EquipModel2 As UInteger
        Dim EquipModel3 As UInteger
        Dim EquipInfo1 As UInteger
        Dim EquipInfo2 As UInteger
        Dim EquipInfo3 As UInteger
        Dim EquipSlot1 As UInteger
        Dim EquipSlot2 As UInteger
        Dim EquipSlot3 As UInteger

        For Each row As DataRow In equipment.Rows
            EquipEntry = CType(row.Item("entry"), Integer)
            EquipModel1 = CType(row.Item("equipmodel1"), Integer)
            EquipModel2 = CType(row.Item("equipmodel2"), Integer)
            EquipModel3 = CType(row.Item("equipmodel3"), Integer)
            EquipInfo1 = CType(row.Item("equipinfo1"), Integer)
            EquipInfo2 = CType(row.Item("equipinfo2"), Integer)
            EquipInfo3 = CType(row.Item("equipinfo3"), Integer)
            EquipSlot1 = CType(row.Item("equipslot1"), Integer)
            EquipSlot2 = CType(row.Item("equipslot2"), Integer)
            EquipSlot3 = CType(row.Item("equipslot3"), Integer)
            SpuriousDB.Update(String.Format("INSERT INTO creatures_equipment (entry, equipmodel1, equipmodel2, equipmodel3, equipinfo1, equipinfo2, equipinfo3, equipslot1, equipslot2, equipslot3) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", EquipEntry, EquipModel1, EquipModel2, EquipModel3, EquipInfo1, EquipInfo2, EquipInfo3, EquipSlot1, EquipSlot2, EquipSlot3))
        Next

        ' Dispose of Database Connections
        AscentDB.Dispose()
        SpuriousDB.Dispose()

        lblStatus.Text = "Conversion Completed - " & result.Rows.Count.ToString & " Creatures Converted."
        Me.Refresh()

    End Sub

    Public Function FixSingle(ByVal Value As Single) As String
        Return Value.ToString.Replace(",", ".")
    End Function
#End Region

#Region "Sells"
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        lblStatus.ForeColor = Color.Black
        ' Set Up and Connect to Ascent Database (for Vendors)
        AscentVendors.SQLTypeServer = SQL.DB_Type.MySQL
        AscentVendors.SQLHost = "localhost"
        AscentVendors.SQLPort = "3306"
        AscentVendors.SQLUser = "root"
        AscentVendors.SQLPass = "root"
        AscentVendors.SQLDBName = "world"
        AscentVendors.Connect()

        ' Set Up and Connect to Spurious Database (for Sells)
        SpuriousSells.SQLTypeServer = SQL.DB_Type.MySQL
        SpuriousSells.SQLHost = "localhost"
        SpuriousSells.SQLPort = "3306"
        SpuriousSells.SQLUser = "root"
        SpuriousSells.SQLPass = "root"
        SpuriousSells.SQLDBName = "spurious"
        SpuriousSells.Connect()

        lblStatus.Text = "Loading the Vendor table from Ascent..."
        Me.Refresh()

        Dim result As New DataTable
        Dim query As String
        query = "SELECT * FROM vendors;"
        AscentDB.Query(query, result)

        lblStatus.Text = "Conversion In Progress...."
        Me.Refresh()
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = result.Rows.Count
        Dim i As Double = 0
        For Each Row As DataRow In result.Rows
            i = i + 1
            ProgressBar1.Value = i
            If (i \ 100) = Int(i \ 100) Then Me.Refresh()
            Vendors.Entry = CType(Row.Item("entry").ToString, Integer)
            Vendors.Item = CType(Row.Item("item").ToString, Integer)
            Vendors.Amount = CType(Row.Item("amount").ToString, Integer)
            Vendors.Inctime = CType(Row.Item("inctime").ToString, Integer)

            ' Insert Information Into Spawns Table On Spurious Database
            SpuriousDB.Update(String.Format("INSERT INTO sells (sell_id, sell_item, sell_amount, sell_group) VALUES ({0}, {1}, {2}, {3}, {4});", Vendors.Entry, Vendors.Item, Vendors.Amount, Vendors.Inctime))
        Next
        ' Dispose of Database Connections
        AscentVendors.Dispose()
        SpuriousSells.Dispose()

        lblStatus.Text = "Conversion Completed - " & result.Rows.Count.ToString & " Sells Converted."
        Me.Refresh()
        'TODO2: Find out what sell_group represents and document this in the wiki
    End Sub
#End Region



End Class
