' Here are listed sctipted area triggers, called on packet recv.
' All not scripted triggers are supposed to be teleport trigers,
' handled by the core.
'
'Last  update: 08.03.2007

Imports System
Imports Microsoft.VisualBasic
Imports MangosVB.WorldServer
Imports Common

Namespace Scripts
	Public Module AreaTriggers

		'Area-52 Neuralyzer
		Public Sub HandleAreaTrigger_4422(ByVal GUID as ULong)
			If CHARACTERs(GUID).HaveAura(34400) = False Then CHARACTERs(GUID).CastOnSelf(34400)
		End Sub
		Public Sub HandleAreaTrigger_4466(ByVal GUID as ULong)
			If CHARACTERs(GUID).HaveAura(34400) = False Then CHARACTERs(GUID).CastOnSelf(34400)
		End Sub
		Public Sub HandleAreaTrigger_4471(ByVal GUID as ULong)
			If CHARACTERs(GUID).HaveAura(34400) = False Then CHARACTERs(GUID).CastOnSelf(34400)
		End Sub
		Public Sub HandleAreaTrigger_4472(ByVal GUID as ULong)
			If CHARACTERs(GUID).HaveAura(34400) = False Then CHARACTERs(GUID).CastOnSelf(34400)
		End Sub



		'These are sample or test triggers
		Public Sub HandleAreaTrigger_1(ByVal GUID as ULong)
			SetFlag(Characters(GUID).cPlayerFlags, PlayerFlag.PLAYER_FLAG_RESTING, True)
		End Sub

		Public Sub HandleAreaTrigger_710(ByVal GUID as ULong)
			SetFlag(Characters(GUID).cPlayerFlags, PlayerFlag.PLAYER_FLAG_RESTING, True)
			Characters(GUID).SetUpdateFlag(EPlayerFields.PLAYER_FLAGS, Characters(GUID).cPlayerFlags)
		End Sub

		Public Sub HandleAreaTrigger_2175(ByVal GUID as ULong)
			Characters(GUID).Teleport(71.1234f, 9.74174f, -4.29735f, Characters(GUID).Orientation, Characters(GUID).MapID)
		End Sub

	End Module
End Namespace
