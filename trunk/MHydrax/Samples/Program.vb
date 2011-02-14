Imports Mogre

Public Module Program

    Public Sub Main()
        Dim root As Root = Nothing
        Try
            Try
                ' Initialize Ogre.
                root = New Root("") ' No plugin filename.

                ' Load appropriate plugins.
#If DEBUG Then
                root.LoadPlugin("Plugin_CgProgramManager_d")
                root.LoadPlugin("Plugin_OctreeSceneManager_d")
                root.LoadPlugin("RenderSystem_Direct3D9_d")
                root.LoadPlugin("RenderSystem_GL_d")
#Else
                root.LoadPlugin("Plugin_CgProgramManager")
                root.LoadPlugin("Plugin_OctreeSceneManager")
                root.LoadPlugin("RenderSystem_Direct3D9")
                root.LoadPlugin("RenderSystem_GL")
#End If

                ' Show Ogre's default config dialog to let the user setup resolution, etc.
                Dim result As Boolean = root.ShowConfigDialog()

                ' If the user clicks OK, continue.
                If (result) Then
                    Dim myDemo As New ProjectedGridDemo(root)
                    Try
                        myDemo.CreateScene() ' Create the scene.
                        myDemo.Run() ' Display the island.
                    Catch exInner As Exception
                        System.Windows.Forms.MessageBox.Show(exInner.ToString(), "Inner exception has occured!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
                    Finally
                        myDemo.Dispose() ' Clean up.
                    End Try
                End If

            Catch ex As System.Runtime.InteropServices.SEHException
                ' Check if it's an Ogre Exception.
                If OgreException.IsThrown Then
                    System.Windows.Forms.MessageBox.Show(OgreException.LastException.FullDescription, "An exception has occured!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
                Else
                    Throw
                End If
            End Try
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.ToString(), "An exception has occured!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
        Finally
            If root IsNot Nothing Then
                ' Shut down Ogre.
                root.Dispose()
                root = Nothing
            End If
        End Try
    End Sub

End Module
