'--------------------------------------------------------------------------------
'This source file is part of MHydrax.
'Visit ---

'Copyright (C) 2009 Christian Haettig

'This program is free software; you can redistribute it and/or modify it under
'the terms of the GNU Lesser General Public License as published by the Free Software
'Foundation; either version 3 of the License, or (at your option) any later
'version.

'This program is distributed in the hope that it will be useful, but WITHOUT
'ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
'FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details.

'You should have received a copy of the GNU Lesser General Public License along with
'this program; if not, write to the Free Software Foundation, Inc., 59 Temple
'Place - Suite 330, Boston, MA 02111-1307, USA, or go to
'http://www.gnu.org/copyleft/lesser.txt.
'--------------------------------------------------------------------------------

' Demo1.vb

Imports Mogre

Public Class ProjectedGridDemo

    Private Const RESOURCES_FILENAME As String = "resources.cfg"
    Private Const AppTitle As String = "MHydrax ProjectedGridDemo"

    Private running As Boolean ' A flag which, when set to false, will terminate a simulation started with Run()

    ' Various pointers to Ogre objects are stored here:
    Private root As Root
    Private window As RenderWindow
    Private viewport As Viewport
    Private sceneMgr As SceneManager
    Private camera As Camera

    ' OIS input objects
    Private inputManager As MOIS.InputManager
    Private keyboard As MOIS.Keyboard
    Private mouse As MOIS.Mouse

    Private mKeyBuffer As Single

    ' Variables used to keep track of the camera's rotation/etc.
    Private camPitch As Radian
    Private camYaw As Radian

    ' Pointer to MHydrax instance.
    Private hydrax As MHydrax.MHydrax

    Private mCurrentSkyBox As Integer = 0

    Private mSkyBoxes As String() = {"Sky/ClubTropicana", _
                                     "Sky/EarlyMorning", _
                                     "Sky/Clouds"}

    Private mSunPosition As Vector3() = {New Vector3(0, 10000, 0), _
                                         New Vector3(0, 10000, 90000), _
                                         New Vector3(0, 10000, 0)}

    Private mSunColor As Vector3() = {New Vector3(1, 0.9, 0.6), _
                                      New Vector3(1, 0.6, 0.4), _
                                      New Vector3(0.45, 0.45, 0.45)}

    ' Just to show skyboxes information.
    Private mTextArea As TextAreaOverlayElement

    ' Debug overlay.
    Private Const PREFIX_currFps As String = "Current FPS: "
    Private Const PREFIX_avgFps As String = "Average FPS: "
    Private Const PREFIX_bestFps As String = "Best FPS: "
    Private Const PREFIX_worstFps As String = "Worst FPS: "
    Private Const PREFIX_tris As String = "Triangle Count: "
    Private Const PREFIX_batches As String = "Batch Count: "
    Private mDebugOverlay As Overlay

    Public Sub New(ByVal r As Root)
        ' Setup Mogre.Root and the scene manager.
        root = r
        window = root.Initialise(True, AppTitle)
        sceneMgr = root.CreateSceneManager("TerrainSceneManager")

        ' Initialize the camera and viewport.
        camera = sceneMgr.CreateCamera("MainCamera")
        viewport = window.AddViewport(camera)
        camera.AspectRatio = CSng(viewport.ActualWidth / viewport.ActualHeight)
        camera.NearClipDistance = 0.1F

        ' Load resources configuration file and register resource locations.
        Dim cf As New ConfigFile
        cf.Load(RESOURCES_FILENAME, "=", True)
        Dim seci As ConfigFile.SectionIterator = cf.GetSectionIterator()
        Dim secName As String
        Dim typeName As String
        Dim archName As String
        While seci.MoveNext()
            secName = seci.CurrentKey
            Dim settings As ConfigFile.SettingsMultiMap = seci.Current
            For Each pair As KeyValuePair(Of String, String) In settings
                typeName = pair.Key
                archName = pair.Value
                ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName)
            Next pair
        End While

        ' Load media.
        ResourceGroupManager.Singleton.InitialiseAllResourceGroups()

        ' Initialize MOIS
        Dim windowHnd As IntPtr
        Dim pl As New MOIS.ParamList
        window.GetCustomAttribute("WINDOW", windowHnd)
        pl.Insert("WINDOW", windowHnd.ToString())
        inputManager = MOIS.InputManager.CreateInputSystem(pl)

        keyboard = DirectCast(inputManager.CreateInputObject(MOIS.Type.OISKeyboard, False), MOIS.Keyboard)
        mouse = DirectCast(inputManager.CreateInputObject(MOIS.Type.OISMouse, False), MOIS.Mouse)

        ' Reset camera orientation.
        camPitch = 0
        camYaw = 0
    End Sub

    Public Sub Dispose()
        ' Shut down MOIS
        inputManager.DestroyInputObject(keyboard)
        keyboard.Dispose()
        keyboard = Nothing
        inputManager.DestroyInputObject(mouse)
        mouse.Dispose()
        mouse = Nothing
        MOIS.InputManager.DestroyInputSystem(inputManager)
        inputManager.Dispose()
        inputManager = Nothing

        Unload()
    End Sub

    Public Sub CreateScene()
        ' Set default ambient light
        sceneMgr.AmbientLight = New ColourValue(1, 1, 1)

        ' Create the SkyBox
        sceneMgr.SetSkyBox(True, mSkyBoxes(mCurrentSkyBox), 99999 * 3, True)

        ' Set some camera params
        camera.FarClipDistance = 99999 * 6
        camera.Position = New Vector3(-255, 200, 6)
        camera.Orientation = New Quaternion(0.998, -0.0121, -0.0608, -0.00074)

        ' Light
        Dim mLight As Light = sceneMgr.CreateLight("Light0")
        mLight.Position = mSunPosition(mCurrentSkyBox)
        mLight.DiffuseColour = New ColourValue(1, 1, 1)
        mLight.SpecularColour = New ColourValue(mSunColor(mCurrentSkyBox).x, _
                                                mSunColor(mCurrentSkyBox).y, _
                                                mSunColor(mCurrentSkyBox).z)

        ' Hydrax initialization code ---------------------------------------------
        ' ------------------------------------------------------------------------

        ' Create Hydrax object
        hydrax = New MHydrax.MHydrax(sceneMgr, camera, window.GetViewport(0))

        ' Set hydrax components.
        hydrax.Components = MHydrax.MHydraxComponent.HYDRAX_COMPONENT_CAUSTICS Or _
                            MHydrax.MHydraxComponent.HYDRAX_COMPONENT_DEPTH Or _
                            MHydrax.MHydraxComponent.HYDRAX_COMPONENT_FOAM Or _
                            MHydrax.MHydraxComponent.HYDRAX_COMPONENT_SMOOTH Or _
                            MHydrax.MHydraxComponent.HYDRAX_COMPONENT_SUN Or _
                            MHydrax.MHydraxComponent.HYDRAX_COMPONENT_UNDERWATER Or _
                            MHydrax.MHydraxComponent.HYDRAX_COMPONENT_UNDERWATER_GODRAYS Or _
                            MHydrax.MHydraxComponent.HYDRAX_COMPONENT_UNDERWATER_REFLECTIONS

        ' Create our projected grid module
        ' Parameters:
        ' Hydrax parent pointer
        ' Noise module
        ' Base plane
        ' Normal mode
        ' Projected grid options
        Dim m As New MHydrax.MProjectedGrid(hydrax, _
                                            New MHydrax.MPerlin(New MHydrax.MPerlin.MOptions(8, 0.085, 0.49, 1.4, 1.27, 2, New Vector3(0.5, 50, 150000))), _
                                            New Plane(New Vector3(0, 1, 0), New Vector3(0, 0, 0)), _
                                            MHydrax.MMaterialManager.MNormalMode.NM_VERTEX, _
                                            New MHydrax.MProjectedGrid.MOptions(256, 35, 50, False, False, True, 3.75))

        ' Set our module
        hydrax.SetModule(m)

        ' Set all parameters instead of loading all parameters from config file:
        'hydrax.LoadCfg("ProjectedGridDemo.hdx")
        ' #Main options
        hydrax.Position = New Vector3(5000, 100, -5000)
        hydrax.PlanesError = 10.5
        hydrax.ShaderMode = MHydrax.MMaterialManager.MShaderMode.SM_HLSL
        hydrax.FullReflectionDistance = 100000000000
        hydrax.GlobalTransparency = 0
        hydrax.NormalDistortion = 0.075
        hydrax.WaterColor = New Vector3(0.139765, 0.359464, 0.425373)
        ' #Sun parameters
        hydrax.SunPosition = New Vector3(0, 10000, 0)
        hydrax.SunStrength = 1.75
        hydrax.SunArea = 150
        hydrax.SunColor = New Vector3(1, 0.9, 0.6)
        ' #Foam parameters
        hydrax.FoamMaxDistance = 75000000
        hydrax.FoamScale = 0.0075
        hydrax.FoamStart = 0
        hydrax.FoamTransparency = 1
        ' #Depth parameters
        hydrax.DepthLimit = 90
        ' #Smooth transitions parameters
        hydrax.SmoothPower = 5
        ' #Caustics parameters
        hydrax.CausticsScale = 135
        hydrax.CausticsPower = 10.5
        hydrax.CausticsEnd = 0.8
        ' #God rays parameters
        hydrax.GodRaysExposure = New Vector3(0.76, 2.46, 2.29)
        hydrax.GodRaysIntensity = 0.015
        hydrax.GodRaysManager.SimulationSpeed = 5
        hydrax.GodRaysManager.NumberOfRays = 100
        hydrax.GodRaysManager.RaysSize = 0.03
        hydrax.GodRaysManager.ObjectsIntersectionsEnabled = False
        ' #Rtt quality field(0x0 = Auto)
        ' TODO: RTTManager not wrapped yet.
        '<size>Rtt_Quality_Reflection=0x0
        '<size>Rtt_Quality_Refraction=0x0
        '<size>Rtt_Quality_Depth=0x0
        '<size>Rtt_Quality_URDepth=0x0
        '<size>Rtt_Quality_GPUNormalMap=0x0

        ' Create water
        hydrax.Create()

        ' Hydrax initialization code end -----------------------------------------
        ' ------------------------------------------------------------------------

        ' Load island
        sceneMgr.SetWorldGeometry("Island.cfg")

        ' Add our hydrax depth technique to island material
        ' (Because the terrain mesh is not an Ogre::Entity)
        hydrax.MaterialManager.AddDepthTechnique( _
                CType(MaterialManager.Singleton.GetByName("Island"), MaterialPtr) _
                .CreateTechnique())

        ' Create palmiers
        CreatePalms(sceneMgr)

        ' Add a decal to the sea.
        Dim d As MHydrax.MDecal = hydrax.DecalsManager.Add("Rosette.png")
        d.Position = New Vector2(120, 120)

        ' Create text area to show skyboxes information.
        CreateTextArea()

        ' Get debug overlay.
        mDebugOverlay = OverlayManager.Singleton.GetByName("Core/DebugOverlay")
        mDebugOverlay.Show()
    End Sub

    ' Just to locate palms with a pseudo-random algoritm
    Private _Seed As Single = 801.0F
    Private Function Rnd(ByVal min As Single, ByVal max As Single) As Single
        _Seed += Mogre.Math.PI * 2.8574F + _Seed * (0.3424F - 0.12434F + 0.452345F)
        If _Seed > 10000000000 Then
            _Seed -= 10000000000
        End If
        Return ((max - min) * Mogre.Math.Abs(Mogre.Math.Sin(New Radian(_Seed))) + min)
    End Function

    Private Sub CreatePalms(ByVal sceneMgr As SceneManager)
        Dim numberOfPalms As Integer = 12

        Dim palmsSceneNode As SceneNode = sceneMgr.RootSceneNode.CreateChildSceneNode()

        For k As Integer = 0 To numberOfPalms Step 1
            Dim randomPos As New Vector3(Rnd(500, 2500), 0, Rnd(500, 2500))

            Dim rsq As RaySceneQuery = sceneMgr.CreateRayQuery( _
                        New Ray(randomPos + New Vector3(0, 1000000, 0), _
                        Vector3.NEGATIVE_UNIT_Y))

            Dim qryResult As RaySceneQueryResult = rsq.Execute()
            Dim it As RaySceneQueryResult.Iterator = qryResult.Begin()

            If it IsNot qryResult.End() AndAlso it.Value.worldFragment IsNot Nothing Then
                If (it.Value.worldFragment.singleIntersection.y > 105 OrElse it.Value.worldFragment.singleIntersection.y < 20) Then
                    k -= 1
                    Continue For
                End If

                randomPos.y = it.Value.worldFragment.singleIntersection.y
            Else
                k -= 1
                Continue For
            End If

            Dim palmEnt As Entity = sceneMgr.CreateEntity("Palm" & k.ToString(), "Palm.mesh")
            Dim palmSN As SceneNode = palmsSceneNode.CreateChildSceneNode()

            palmSN.Rotate(New Vector3(-1, 0, Rnd(-0.3, 0.3)), New Degree(90))
            palmSN.AttachObject(palmEnt)
            Dim scale As Single = Rnd(50, 75)
            palmSN.Scale(scale, scale, scale)
            palmSN.Position = randomPos
        Next k
    End Sub

    ' Create text area to show skyboxes information.
    Private Sub CreateTextArea()
        ' Create a panel.
        Dim panel As Mogre.OverlayContainer = DirectCast(OverlayManager.Singleton.CreateOverlayElement("Panel", "HydraxDemoInformationPanel"), OverlayContainer)
        panel.MetricsMode = GuiMetricsMode.GMM_PIXELS
        panel.SetPosition(10, 10)
        panel.SetDimensions(400, 400)

        ' Create a text area.
        mTextArea = DirectCast(OverlayManager.Singleton.CreateOverlayElement("TextArea", "HydraxDemoInformationTextArea"), TextAreaOverlayElement)
        mTextArea.MetricsMode = GuiMetricsMode.GMM_PIXELS
        mTextArea.SetPosition(0, 0)
        mTextArea.SetDimensions(100, 100)
        mTextArea.CharHeight = 16
        mTextArea.Caption = "Hydrax 0.5 demo application" & Environment.NewLine & "Current water preset: " & mSkyBoxes(mCurrentSkyBox).Split("/"c)(1) & " (" & (mCurrentSkyBox + 1).ToString() & "/3). Press 'm' to switch water presets."
        mTextArea.FontName = "BlueHighway"
        mTextArea.ColourBottom = New ColourValue(0.3, 0.5, 0.3)
        mTextArea.ColourTop = New ColourValue(0.5, 0.7, 0.5)

        ' Create an overlay and add the panel.
        Dim overlay As Overlay = OverlayManager.Singleton.Create("OverlayName")
        overlay.Add2D(panel)

        ' Add the text area to the panel.
        panel.AddChild(mTextArea)

        ' Show the overlay.
        overlay.Show()
    End Sub

    Private Sub UpdateStats()
        Dim guiAvg As OverlayElement = OverlayManager.Singleton.GetOverlayElement("Core/AverageFps")
        Dim guiCurr As OverlayElement = OverlayManager.Singleton.GetOverlayElement("Core/CurrFps")
        Dim guiBest As OverlayElement = OverlayManager.Singleton.GetOverlayElement("Core/BestFps")
        Dim guiWorst As OverlayElement = OverlayManager.Singleton.GetOverlayElement("Core/WorstFps")
        Dim stats As RenderTarget.FrameStats = window.GetStatistics
        guiAvg.Caption = PREFIX_avgFps & stats.AvgFPS.ToString("#.00")
        guiCurr.Caption = PREFIX_currFps & stats.LastFPS.ToString("#.00")
        guiBest.Caption = PREFIX_bestFps & stats.BestFPS.ToString("#.00") & " " & stats.BestFrameTime & " ms"
        guiWorst.Caption = PREFIX_worstFps & stats.WorstFPS.ToString("#.00") & " " & stats.WorstFrameTime & " ms"
        OverlayManager.Singleton.GetOverlayElement("Core/NumTris").Caption = PREFIX_tris & stats.TriangleCount
        OverlayManager.Singleton.GetOverlayElement("Core/NumBatches").Caption = PREFIX_batches & stats.BatchCount
    End Sub

    Private Sub ChangeSkyBox()
        ' Change skybox
        sceneMgr.SetSkyBox(True, mSkyBoxes(mCurrentSkyBox), 99999 * 3, True)

        ' Update Hydrax sun position and colour
        hydrax.SunPosition = mSunPosition(mCurrentSkyBox)
        hydrax.SunColor = mSunColor(mCurrentSkyBox)

        ' Update light 0 light position and colour
        sceneMgr.GetLight("Light0").SetPosition(mSunPosition(mCurrentSkyBox).x, _
                                                mSunPosition(mCurrentSkyBox).y, _
                                                mSunPosition(mCurrentSkyBox).z)
        sceneMgr.GetLight("Light0").SetSpecularColour(mSunColor(mCurrentSkyBox).x, _
                                                      mSunColor(mCurrentSkyBox).y, _
                                                      mSunColor(mCurrentSkyBox).z)

        ' Update text area.
        mTextArea.Caption = "Hydrax 0.5 demo application" & Environment.NewLine & "Current water preset: " & mSkyBoxes(mCurrentSkyBox).Split("/"c)(1) & " (" & (mCurrentSkyBox + 1).ToString() & "/3). Press 'm' to switch water presets."

        LogManager.Singleton.LogMessage("Skybox " & mSkyBoxes(mCurrentSkyBox) & " selected. (" & (mCurrentSkyBox + 1).ToString() & "/" & mSkyBoxes.Length.ToString() & ")")
    End Sub

    ''' <summary>
    ''' Unloads the 3D scene cleanly.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Unload()
        ' [NOTE] Always remember to dispose any MHydrax instances in order to avoid memory leaks and AccessViolationExceptions.

        ' Delete the MHydrax instance.
        hydrax.Dispose()
        hydrax = Nothing
    End Sub

    ''' <summary>
    ''' Runs the simulation.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Run()
        AddHandler root.FrameEnded, AddressOf FrameEnded

        ' Render loop
        running = True
        While (running)
            ' Handle windows events.
            WindowEventUtilities.MessagePump()

            ' Render the scene with Ogre.
            root.RenderOneFrame()

            ' Exit immediately if the window is closed.
            If window.IsClosed() Then
                Exit While
            End If
        End While

        RemoveHandler root.FrameEnded, AddressOf FrameEnded
    End Sub

    Private Function FrameEnded(ByVal evt As FrameEvent) As Boolean
        ProcessInput(evt.timeSinceLastFrame)

        ' Update Hydrax (After any camera position/orientation/... change)
        hydrax.Update(evt.timeSinceLastFrame)

        UpdateStats()

        Return True
    End Function

    ''' <summary>
    ''' Accepts keyboard and mouse input, allowing you to move around in the world.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessInput(ByVal timeSinceLastFrame As Single)
        ' Get the current state of the keyboard and mouse
        keyboard.Capture()
        mouse.Capture()

        ' Always exit if ESC is pressed
        If keyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE) Then
            running = False
        End If

        ' Check for switch water presets

        If keyboard.IsKeyDown(MOIS.KeyCode.KC_M) And mKeyBuffer < 0 Then
            mCurrentSkyBox += 1

            If mCurrentSkyBox > mSkyBoxes.Length - 1 Then
                mCurrentSkyBox = 0
            End If
            ChangeSkyBox()

            mKeyBuffer = 0.5F
        End If

        mKeyBuffer -= timeSinceLastFrame

        ' Get mouse movement
        Dim ms As MOIS.MouseState_NativePtr = mouse.MouseState

        ' Update camera rotation based on the mouse
        Dim cameraYaw As Degree = (-ms.X.rel * 0.13F)
        Dim cameraPitch As Degree = (-ms.Y.rel * 0.13F)
        Me.camera.Yaw(cameraYaw)
        Me.camera.Pitch(cameraPitch)

        ' Allow the camera to move around with the arrow/WASD keys
        Dim translateVector As New Vector3
        If (keyboard.IsKeyDown(MOIS.KeyCode.KC_UP) OrElse keyboard.IsKeyDown(MOIS.KeyCode.KC_W)) Then
            translateVector.z = -1
        End If
        If (keyboard.IsKeyDown(MOIS.KeyCode.KC_DOWN) OrElse keyboard.IsKeyDown(MOIS.KeyCode.KC_S)) Then
            translateVector.z = 1
        End If
        If (keyboard.IsKeyDown(MOIS.KeyCode.KC_RIGHT) OrElse keyboard.IsKeyDown(MOIS.KeyCode.KC_D)) Then
            translateVector.x = 1
        End If
        If (keyboard.IsKeyDown(MOIS.KeyCode.KC_LEFT) OrElse keyboard.IsKeyDown(MOIS.KeyCode.KC_A)) Then
            translateVector.x = -1
        End If
        If (keyboard.IsKeyDown(MOIS.KeyCode.KC_PGUP) OrElse keyboard.IsKeyDown(MOIS.KeyCode.KC_E)) Then
            translateVector.y = 1
        End If
        If (keyboard.IsKeyDown(MOIS.KeyCode.KC_PGDOWN) OrElse keyboard.IsKeyDown(MOIS.KeyCode.KC_Q)) Then
            translateVector.y = -1
        End If

        ' Shift = speed boost
        If (keyboard.IsKeyDown(MOIS.KeyCode.KC_LSHIFT) OrElse keyboard.IsKeyDown(MOIS.KeyCode.KC_RSHIFT)) Then
            translateVector *= 2
        End If

        translateVector *= 700 ' Move speed.
        camera.MoveRelative(translateVector * timeSinceLastFrame)
    End Sub

End Class
