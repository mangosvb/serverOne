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
Public Class AscentQuests

#Region "Constants"
    'Completed This Part... Finally...
    Public v_entry As Integer = 0
    Public v_ZoneId As Integer = 0
    Public v_sort As Integer = 0
    Public v_flags As Integer = 0
    Public v_MinLevel As Integer = 0
    Public v_questlevel As Integer = 0
    Public v_Type As Integer = 0
    Public v_RequiredRaces As Integer = 0
    Public v_RequiredClass As Integer = 0
    Public v_RequiredTradeskill As Integer = 0
    Public v_RequiredTradeskillValue As Integer = 0
    Public v_RequiredRepFaction As Integer = 0
    Public v_RequiredRepValue As Integer = 0
    Public v_LimitTime As Integer = 0
    Public v_SpecialFlags As Integer = 0
    Public v_PrevQuestId As Integer = 0
    Public v_NextQuestId As Integer = 0
    Public v_srcItem As Integer = 0
    Public v_SrcItemCount As Integer = 0
    Public v_Title As String = ""
    Public v_Details As String = ""
    Public v_Objectives As String = ""
    Public v_CompletionText As String = ""
    Public v_IncompleteText As String = ""
    Public v_EndText As String = ""
    Public v_ObjectiveText1 As String = ""
    Public v_ObjectiveText2 As String = ""
    Public v_ObjectiveText3 As String = ""
    Public v_ObjectiveText4 As String = ""
    Public v_ReqItemId1 As Integer = 0
    Public v_ReqItemId2 As Integer = 0
    Public v_ReqItemId3 As Integer = 0
    Public v_ReqItemId4 As Integer = 0
    Public v_ReqItemCount1 As Integer = 0
    Public v_ReqItemCount2 As Integer = 0
    Public v_ReqItemCount3 As Integer = 0
    Public v_ReqItemCount4 As Integer = 0
    Public v_ReqKillMoborGOId1 As Integer = 0
    Public v_ReqKillMoborGOId2 As Integer = 0
    Public v_ReqKillMoborGOId3 As Integer = 0
    Public v_ReqKillMoborGOId4 As Integer = 0
    Public v_ReqKillMoborGOCount1 As Integer = 0
    Public v_ReqKillMoborGOCount2 As Integer = 0
    Public v_ReqKillMoborGOCount3 As Integer = 0
    Public v_ReqKillMoborGOCount4 As Integer = 0
    Public v_ReqCastSpellId1 As Integer = 0
    Public v_ReqCastSpellId2 As Integer = 0
    Public v_ReqCastSpellId3 As Integer = 0
    Public v_ReqCastSpellId4 As Integer = 0
    Public v_RewChoiceItemId1 As Integer = 0
    Public v_RewChoiceItemId2 As Integer = 0
    Public v_RewChoiceItemId3 As Integer = 0
    Public v_RewChoiceItemId4 As Integer = 0
    Public v_RewChoiceItemId5 As Integer = 0
    Public v_RewChoiceItemId6 As Integer = 0
    Public v_RewChoiceItemCount1 As Integer = 0
    Public v_RewChoiceItemCount2 As Integer = 0
    Public v_RewChoiceItemCount3 As Integer = 0
    Public v_RewChoiceItemCount4 As Integer = 0
    Public v_RewChoiceItemCount5 As Integer = 0
    Public v_RewChoiceItemCount6 As Integer = 0
    Public v_RewItemId1 As Integer = 0
    Public v_RewItemId2 As Integer = 0
    Public v_ReqItemId3 As Integer = 0
    Public v_ReqItemId4 As Integer = 0
    Public v_RewItemCount1 As Integer = 0
    Public v_RewItemCount2 As Integer = 0
    Public v_RewItemCount3 As Integer = 0
    Public v_RewItemCount4 As Integer = 0
    Public v_RewRepFaction1 As Integer = 0
    Public v_RewRepFaction2 As Integer = 0
    Public v_RewRepValue1 As Integer = 0
    Public v_ReqRepValue2 As Integer = 0
    Public v_RewRepLimit As Integer = 0
    Public v_RewMoney As Integer = 0
    Public v_RewXP As Integer = 0
    Public v_RewSpell As Integer = 0
    Public v_CastSpell As Integer = 0
    Public v_PointMapId As Integer = 0
    Public v_PointX As Integer = 0
    Public v_PointY As Integer = 0
    Public v_PointOpt As Integer = 0
    Public v_RequiredMoney As Integer = 0
    Public v_ExploreTrigger1 As Integer = 0
    Public v_ExploreTrigger2 As Integer = 0
    Public v_ExploreTrigger3 As Integer = 0
    Public v_ExploreTrigger4 As Integer = 0
    Public v_RequiredQuest1 As Integer = 0
    Public v_RequiredQuest2 As Integer = 0
    Public v_RequiredQuest3 As Integer = 0
    Public v_RequiredQuest4 As Integer = 0
    Public v_RecieveItemId1 As Integer = 0
    Public v_RecieveItemId2 As Integer = 0
    Public v_RecieveItemId3 As Integer = 0
    Public v_RecieveItemId4 As Integer = 0
    Public v_RecieveItemCount1 As Integer = 0
    Public v_RecieveItemCount2 As Integer = 0
    Public v_RecieveItemCount3 As Integer = 0
    Public v_RecieveItemCount4 As Integer = 0
    Public v_IsRepeatable As Integer = 0
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

    Public Property ZoneId() As Integer
        Get
            ZoneId = v_ZoneId
        End Get
        Set(ByVal Value As Integer)
            v_ZoneId = Value
        End Set
    End Property

    Public Property Sort() As Integer
        Get
            Sort = v_sort
        End Get
        Set(ByVal Value As Integer)
            v_sort = Value
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
            MinLevel = v_MinLevel
        End Get
        Set(ByVal Value As Integer)
            v_MinLevel = Value
        End Set
    End Property

    Public Property QuestLevel() As Integer
        Get
            QuestLevel = v_Quest
        End Get
        Set(ByVal Value As Integer)
            v_QuestLevel = Value
        End Set
    End Property
    Public Property Type() As Integer
        Get
            QuestLevel = v_Type
        End Get
        Set(ByVal Value As Integer)
            v_Type = Value
        End Set
    End Property
    Public Property RequiredRaces() As Integer
        Get
            RequiredRaces = v_RequiredRaces
        End Get
        Set(ByVal Value As Integer)
            v_RequiredRaces = Value
        End Set
    End Property
    Public Property RequiredClass() As Integer
        Get
            RequiredClass = v_RequiredClass
        End Get
        Set(ByVal Value As Integer)
            v_RequiredClass = Value
        End Set
    End Property
    Public Property RequiredTradeSkill() As Integer
        Get
            RequiredTradeSkill = v_RequiredTradeSkill
        End Get
        Set(ByVal Value As Integer)
            v_RequiredTradeSkill = Value
        End Set
    End Property
    Public Property RequiredTradeSkillValue() As Integer
        Get
            RequiredTradeSkillValue = v_RequiredTradeSkillValue
        End Get
        Set(ByVal Value As Integer)
            v_RequiredTradeSkillValue = Value
        End Set
    End Property
    Public Property RequiredRepFaction() As Integer
        Get
            RequiredRepFaction = v_RequiredRepFaction
        End Get
        Set(ByVal Value As Integer)
            v_RequiredRepFaction = Value
        End Set
    End Property
    Public Property RequiredRepValue() As Integer
        Get
            RequiredRepValue = v_RequiredRepValue
        End Get
        Set(ByVal Value As Integer)
            v_RequiredRepValue = Value
        End Set
    End Property
    Public Property LimitTime() As Integer
        Get
            LimitTime = v_LimitTime
        End Get
        Set(ByVal Value As Integer)
            v_LimitTime = Value
        End Set
    End Property
    Public Property SpecialFlags() As Integer
        Get
            SpecialFlags = v_SpecialFlags
        End Get
        Set(ByVal Value As Integer)
            v_SpecialFlags = Value
        End Set
    End Property
    Public Property PrevQuestId() As Integer
        Get
            PrevQuestId = v_PrevQuestId
        End Get
        Set(ByVal Value As Integer)
            v_PrevQuestId = Value
        End Set
    End Property
    Public Property NextQuestId() As Integer
        Get
            NextQuestId = v_NextQuestId
        End Get
        Set(ByVal Value As Integer)
            v_NextQuestId = Value
        End Set
    End Property
    Public Property srcItem() As Integer
        Get
            srcItem = v_srcItem
        End Get
        Set(ByVal Value As Integer)
            v_srcItem = Value
        End Set
    End Property
    Public Property SrcItemCount() As Integer
        Get
            SrcItemCount = v_SrcItemCount
        End Get
        Set(ByVal Value As Integer)
            v_SrcItemCount = Value
        End Set
    End Property
    Public Property Title() As Integer
        Get
            Title = v_Title
        End Get
        Set(ByVal Value As Integer)
            Title = Value
        End Set
    End Property
    Public Property Details() As Integer
        Get
            Details = v_Details
        End Get
        Set(ByVal Value As Integer)
            v_Details = Value
        End Set
    End Property
    Public Property Objectives() As Integer
        Get
            Objectives = v_Objectives
        End Get
        Set(ByVal Value As Integer)
            v_Objectives = Value
        End Set
    End Property
    Public Property CompletionText() As Integer
        Get
            CompletionText = v_CompletionText
        End Get
        Set(ByVal Value As Integer)
            v_CompletionText = Value
        End Set
    End Property
    Public Property IncompleteText() As Integer
        Get
            IncompleteText = v_IncompleteText
        End Get
        Set(ByVal Value As Integer)
            v_IncompleteText = Value
        End Set
    End Property
    Public Property EndText() As Integer
        Get
            EndText = v_EndText
        End Get
        Set(ByVal Value As Integer)
            v_EndText = Value
        End Set
    End Property
    Public Property ObjectiveText1() As Integer
        Get
            ObjectiveText1 = v_ObjectiveText1
        End Get
        Set(ByVal Value As Integer)
            v_ObjectiveText1 = Value
        End Set
    End Property
    Public Property ObjectiveText2() As Integer
        Get
            ObjectiveText2 = v_ObjectiveText2
        End Get
        Set(ByVal Value As Integer)
            v_ObjectiveText2 = Value
        End Set
    End Property
    Public Property ObjectiveText3() As Integer
        Get
            ObjectiveText3 = v_ObjectiveText3
        End Get
        Set(ByVal Value As Integer)
            v_ObjectiveText3 = Value
        End Set
    End Property
    Public Property ObjectiveText4() As Integer
        Get
            ObjectiveText4 = v_ObjectiveText4
        End Get
        Set(ByVal Value As Integer)
            v_ObjectiveText4 = Value
        End Set
    End Property
    Public Property ReqItemId1() As Integer
        Get
            ReqItemId1 = v_ReqItemId1
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemId1 = Value
        End Set
    End Property
    Public Property ReqItemId2() As Integer
        Get
            ReqItemId2 = ReqItemId2
        End Get
        Set(ByVal Value As Integer)
            ReqItemId2 = Value
        End Set
    End Property
    Public Property ReqItemId3() As Integer
        Get
            ReqItemId3 = v_ReqItemId3
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemId3 = Value
        End Set
    End Property
    Public Property ReqItemId4() As Integer
        Get
            ReqItemId4 = v_ReqItemId4
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemId4 = Value
        End Set
    End Property
    Public Property ReqItemCount1() As Integer
        Get
            ReqItemCount1 = v_ReqItemCount1
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemCount1 = Value
        End Set
    End Property
    Public Property ReqItemCount2() As Integer
        Get
            ReqItemCount2 = v_ReqItemCount2
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemCount2 = Value
        End Set
    End Property
    Public Property ReqItemCount3() As Integer
        Get
            ReqItemCount3 = v_ReqItemCount3
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemCount3 = Value
        End Set
    End Property
    Public Property ReqItemCount4() As Integer
        Get
            ReqItemCount4 = v_ReqItemCount4
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemCount4 = Value
        End Set
    End Property
    Public Property ReqItemId4() As Integer
        Get
            ReqItemId4 = v_ReqItemId4
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemId4 = Value
        End Set
    End Property
    Public Property ReqKillMoborGOId1() As Integer
        Get
            ReqKillMoborGOId1 = v_ReqKillMoborGOId1
        End Get
        Set(ByVal Value As Integer)
            v_ReqKillMoborGOId1 = Value
        End Set
    End Property
    Public Property ReqKillMoborGOId2() As Integer
        Get
            ReqKillMoborGOId2 = v_ReqKillMoborGOId2
        End Get
        Set(ByVal Value As Integer)
            v_ReqKillMoborGOId2 = Value
        End Set
    End Property
    Public Property ReqKillMoborGOId3() As Integer
        Get
            ReqKillMoborGOId3 = v_ReqKillMoborGOId3
        End Get
        Set(ByVal Value As Integer)
            v_ReqKillMoborGOId3 = Value
        End Set
    End Property
    Public Property ReqKillMoborGOId4() As Integer
        Get
            ReqKillMoborGOId4 = v_ReqKillMoborGOId4
        End Get
        Set(ByVal Value As Integer)
            v_ReqKillMoborGOId4 = Value
        End Set
    End Property
    Public Property ReqKillMoborGOCount1() As Integer
        Get
            ReqKillMoborGOCount1 = v_ReqKillMoborGOCount1
        End Get
        Set(ByVal Value As Integer)
            v_ReqKillMoborGOCount1 = Value
        End Set
    End Property
    Public Property ReqKillMoborGOCount2() As Integer
        Get
            ReqKillMoborGOCount2 = v_ReqKillMoborGOCount2
        End Get
        Set(ByVal Value As Integer)
            v_ReqKillMoborGOCount2 = Value
        End Set
    End Property
    Public Property ReqKillMoborGOCount3() As Integer
        Get
            ReqKillMoborGOCount3 = v_ReqKillMoborGOCount3
        End Get
        Set(ByVal Value As Integer)
            v_ReqKillMoborGOCount3 = Value
        End Set
    End Property
    Public Property ReqKillMoborGOCount4() As Integer
        Get
            ReqKillMoborGOCount4 = v_ReqKillMoborGOCount4
        End Get
        Set(ByVal Value As Integer)
            v_ReqKillMoborGOCount4 = Value
        End Set
    End Property
    Public Property ReqCastSpellId1() As Integer
        Get
            ReqCastSpellId1 = v_ReqCastSpellId1
        End Get
        Set(ByVal Value As Integer)
            v_ReqCastSpellId1 = Value
        End Set
    End Property
    Public Property ReqCastSpellId2() As Integer
        Get
            ReqCastSpellId2 = v_ReqCastSpellId2
        End Get
        Set(ByVal Value As Integer)
            v_ReqCastSpellId2 = Value
        End Set
    End Property
    Public Property ReqCastSpellId3() As Integer
        Get
            ReqCastSpellId3 = v_ReqCastSpellId3
        End Get
        Set(ByVal Value As Integer)
            v_ReqCastSpellId3 = Value
        End Set
    End Property
    Public Property ReqCastSpellId4() As Integer
        Get
            ReqCastSpellId4 = v_ReqCastSpellId4
        End Get
        Set(ByVal Value As Integer)
            v_ReqCastSpellId4 = Value
        End Set
    End Property
    Public Property RewChoiceItemId1() As Integer
        Get
            RewChoiceItemId1 = RewChoiceItemId1
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemId1 = Value
        End Set
    End Property
    Public Property RewChoiceItemId2() As Integer
        Get
            RewChoiceItemId2 = v_RewChoiceItemId2
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemId2 = Value
        End Set
    End Property
    Public Property RewChoiceItemId3() As Integer
        Get
            RewChoiceItemId3 = v_RewChoiceItemId3
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemId3 = Value
        End Set
    End Property
    Public Property RewChoiceItemId4() As Integer
        Get
            RewChoiceItemId4 = v_RewChoiceItemId4
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemId4 = Value
        End Set
    End Property
    Public Property RewChoiceItemId5() As Integer
        Get
            RewChoiceItemId5 = v_RewChoiceItemId5
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemId5 = Value
        End Set
    End Property
    Public Property RewChoiceItemId6() As Integer
        Get
            RewChoiceItemId6 = v_RewChoiceItemId6
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemId6 = Value
        End Set
    End Property
    Public Property RewChoiceItemCount1() As Integer
        Get
            RewChoiceItemCount1 = v_RewChoiceItemCount1
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemCount1 = Value
        End Set
    End Property
    Public Property RewChoiceItemCount2() As Integer
        Get
            RewChoiceItemCount2 = v_RewChoiceItemCount2
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemCount2 = Value
        End Set
    End Property
    Public Property RewChoiceItemCount3() As Integer
        Get
            RewChoiceItemCount3 = v_RewChoiceItemCount3
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemCount3 = Value
        End Set
    End Property
    Public Property RewChoiceItemCount4() As Integer
        Get
            RewChoiceItemCount4 = v_RewChoiceItemCount4
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemCount4 = Value
        End Set
    End Property
    Public Property RewChoiceItemCount5() As Integer
        Get
            RewChoiceItemCount5 = v_RewChoiceItemCount5
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemCount5 = Value
        End Set
    End Property
    Public Property RewChoiceItemCount6() As Integer
        Get
            RewChoiceItemCount6 = v_RewChoiceItemCount6
        End Get
        Set(ByVal Value As Integer)
            v_RewChoiceItemCount6 = Value
        End Set
    End Property
    Public Property RewItemId1() As Integer
        Get
            RewItemId1 = v_RewItemId1
        End Get
        Set(ByVal Value As Integer)
            v_RewItemId1 = Value
        End Set
    End Property
    Public Property RewItemId2() As Integer
        Get
            RewItemId2 = v_RewItemId2
        End Get
        Set(ByVal Value As Integer)
            v_RewItemId2 = Value
        End Set
    End Property
    Public Property ReqItemId3() As Integer
        Get
            ReqItemId3 = v_ReqItemId3
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemId3 = Value
        End Set
    End Property
    Public Property ReqItemId4() As Integer
        Get
            ReqItemId4 = v_ReqItemId4
        End Get
        Set(ByVal Value As Integer)
            v_ReqItemId4 = Value
        End Set
    End Property
    Public Property RewItemCount1() As Integer
        Get
            RewItemCount1 = v_RewItemCount1
        End Get
        Set(ByVal Value As Integer)
            RewItemCount1 = Value
        End Set
    End Property
    Public Property RewItemCount2() As Integer
        Get
            RewItemCount2 = v_ReqItemId4
        End Get
        Set(ByVal Value As Integer)
            v_RewItemCount2 = Value
        End Set
    End Property
    Public Property RewItemCount3() As Integer
        Get
            RewItemCount3 = v_RewItemCount3
        End Get
        Set(ByVal Value As Integer)
            v_RewItemCount3 = Value
        End Set
    End Property
    Public Property RewItemCount4() As Integer
        Get
            RewItemCount4 = v_RewItemCount4
        End Get
        Set(ByVal Value As Integer)
            v_RewItemCount4 = Value
        End Set
    End Property
    Public Property RewRepFaction1() As Integer
        Get
            RewRepFaction1 = v_RewRepFaction1
        End Get
        Set(ByVal Value As Integer)
            v_RewRepFaction1 = Value
        End Set
    End Property
    Public Property RewRepFaction2() As Integer
        Get
            RewRepFaction2 = v_RewRepFaction2
        End Get
        Set(ByVal Value As Integer)
            v_RewRepFaction2 = Value
        End Set
    End Property
    Public Property RewRepValue1() As Integer
        Get
            RewRepValue1 = v_RewRepValue1
        End Get
        Set(ByVal Value As Integer)
            v_RewRepValue1 = Value
        End Set
    End Property
    Public Property ReqRepValue2() As Integer
        Get
            ReqRepValue2 = v_ReqRepValue2
        End Get
        Set(ByVal Value As Integer)
            v_ReqRepValue2 = Value
        End Set
    End Property
    Public Property RewRepLimit() As Integer
        Get
            RewRepLimit = v_RewRepLimit
        End Get
        Set(ByVal Value As Integer)
            v_RewRepLimit = Value
        End Set
    End Property
    Public Property RewMoney() As Integer
        Get
            RewMoney = v_RewMoney
        End Get
        Set(ByVal Value As Integer)
            v_RewMoney = Value
        End Set
    End Property
    Public Property RewXP() As Integer
        Get
            RewXP = v_RewXP
        End Get
        Set(ByVal Value As Integer)
            v_RewXP = Value
        End Set
    End Property
    Public Property RewSpell() As Integer
        Get
            RewSpell = v_RewSpell
        End Get
        Set(ByVal Value As Integer)
            v_RewSpell = Value
        End Set
    End Property
    Public Property CastSpell() As Integer
        Get
            CastSpell = v_CastSpell
        End Get
        Set(ByVal Value As Integer)
            v_CastSpell = Value
        End Set
    End Property

    Public Property PointMapId() As Integer
        Get
            PointMapId = v_PointMapId
        End Get
        Set(ByVal Value As Integer)
            v_PointMapId = Value
        End Set
    End Property
    Public Property PointX() As Integer
        Get
            PointX = v_PointX
        End Get
        Set(ByVal Value As Integer)
            v_PointX = Value
        End Set
    End Property
    Public Property PointY() As Integer
        Get
            PointY = v_PointY
        End Get
        Set(ByVal Value As Integer)
            v_PointY = Value
        End Set
    End Property
    Public Property PointOpt() As Integer
        Get
            PointOpt = v_PointOpt
        End Get
        Set(ByVal Value As Integer)
            v_PointOpt = Value
        End Set
    End Property
    Public Property RequiredMoney() As Integer
        Get
            RequiredMoney = v_RequiredMoney
        End Get
        Set(ByVal Value As Integer)
            v_RequiredMoney = Value
        End Set
    End Property
    Public Property ExploreTrigger1() As Integer
        Get
            ExploreTrigger1 = v_ExploreTrigger1
        End Get
        Set(ByVal Value As Integer)
            v_ExploreTrigger1 = Value
        End Set
    End Property
    Public Property ExploreTrigger2() As Integer
        Get
            ExploreTrigger2 = v_ExploreTrigger2
        End Get
        Set(ByVal Value As Integer)
            v_ExploreTrigger2 = Value
        End Set
    End Property
    Public Property ExploreTrigger3() As Integer
        Get
            ExploreTrigger3 = v_ExploreTrigger3
        End Get
        Set(ByVal Value As Integer)
            v_ExploreTrigger3 = Value
        End Set
    End Property

    Public Property ExploreTrigger4() As Integer
        Get
            ExploreTrigger4 = v_ExploreTrigger4
        End Get
        Set(ByVal Value As Integer)
            v_ExploreTrigger4 = Value
        End Set
    End Property
    Public Property RequiredQuest1() As Integer
        Get
            RequiredQuest1 = v_RequiredQuest1
        End Get
        Set(ByVal Value As Integer)
            v_RequiredQuest1 = Value
        End Set
    End Property
    Public Property RequiredQuest2() As Integer
        Get
            RequiredQuest2 = v_RequiredQuest2
        End Get
        Set(ByVal Value As Integer)
            v_RequiredQuest2 = Value
        End Set
    End Property
    Public Property RequiredQuest3() As Integer
        Get
            RequiredQuest3 = v_RequiredQuest3
        End Get
        Set(ByVal Value As Integer)
            v_RequiredQuest3 = Value
        End Set
    End Property
    Public Property RequiredQuest4() As Integer
        Get
            RequiredQuest4 = v_RequiredQuest4
        End Get
        Set(ByVal Value As Integer)
            v_RequiredQuest4 = Value
        End Set
    End Property
    Public Property RecieveItemId1() As Integer
        Get
            RecieveItemId1 = v_RecieveItemId1
        End Get
        Set(ByVal Value As Integer)
            v_RecieveItemId1 = Value
        End Set
    End Property
    Public Property RecieveItemId2() As Integer
        Get
            RecieveItemId2 = v_RecieveItemId2
        End Get
        Set(ByVal Value As Integer)
            v_RecieveItemId2 = Value
        End Set
    End Property
    Public Property RecieveItemId3() As Integer
        Get
            RecieveItemId3 = v_RecieveItemId3
        End Get
        Set(ByVal Value As Integer)
            v_RecieveItemId3 = Value
        End Set
    End Property
    Public Property RecieveItemId4() As Integer
        Get
            RecieveItemId4 = v_RecieveItemId4
        End Get
        Set(ByVal Value As Integer)
            v_RecieveItemId4 = Value
        End Set
    End Property
    Public Property RecieveItemCount1() As Integer
        Get
            RecieveItemCount1 = v_RecieveItemCount1
        End Get
        Set(ByVal Value As Integer)
            v_RecieveItemCount1 = Value
        End Set
    End Property
    Public Property RecieveItemCount2() As Integer
        Get
            RecieveItemCount2 = v_RecieveItemCount2
        End Get
        Set(ByVal Value As Integer)
            v_RecieveItemCount2 = Value
        End Set
    End Property
    Public Property RecieveItemCount3() As Integer
        Get
            RecieveItemCount3 = v_RecieveItemCount3
        End Get
        Set(ByVal Value As Integer)
            v_RecieveItemCount3 = Value
        End Set
    End Property
    Public Property RecieveItemCount4() As Integer
        Get
            RecieveItemCount4 = v_RecieveItemCount4
        End Get
        Set(ByVal Value As Integer)
            v_RecieveItemCount4 = Value
        End Set
    End Property
    Public Property IsRepeatable() As Integer
        Get
            IsRepeatable = v_IsRepeatable
        End Get
        Set(ByVal Value As Integer)
            v_IsRepeatable = Value
        End Set
    End Property
#End Region

End Class