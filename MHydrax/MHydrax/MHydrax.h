/*
--------------------------------------------------------------------------------
This source file is part of MHydrax.
Visit ---

Copyright (C) 2009 Christian Haettig

This program is free software; you can redistribute it and/or modify it under
the terms of the GNU Lesser General Public License as published by the Free Software
Foundation; either version 3 of the License, or (at your option) any later
version.

This program is distributed in the hope that it will be useful, but WITHOUT
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with
this program; if not, write to the Free Software Foundation, Inc., 59 Temple
Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/lesser.txt.
--------------------------------------------------------------------------------
*/

// MHydrax.h

#pragma once

namespace MHydrax {

	/// <summary>
	/// Main MHydrax class. 
	/// Hydrax is a plugin for the Ogre3D engine whose aim is rendering realistic water scenes.
	///	Do not use two instances of the MHydrax class.
	/// </summary>
	public ref class MHydrax : System::IDisposable {
	private:
		Hydrax::Hydrax* internalHydrax;

		MModule^ mModule;

		// Track whether Dispose has been called.
		bool disposed;

	public:
		/// <summary>
		/// Conversion operator to type Hydrax::Hydrax*.
		/// </summary>
		operator Hydrax::Hydrax *() {
			if(!disposed) {
				return internalHydrax;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
        /// Creates a new instance of MHydrax.
		/// </summary>
		/// <param name="sceneMgr">Mogre SceneManager.</param>
		/// <param name="cam">Mogre Camera.</param>
		/// <param name="vp">Mogre Main window viewport.</param>
		///	<remarks>Do not use two instances of the MHydrax class.</remarks>
		MHydrax(Mogre::SceneManager ^sceneMgr, Mogre::Camera ^cam, Mogre::Viewport ^vp) {
			internalHydrax = new Hydrax::Hydrax((Ogre::SceneManager*)sceneMgr, (Ogre::Camera*)cam, (Ogre::Viewport*)vp);
			mModule = nullptr;
		}

		/// <summary>
        /// Creates all resources according to current MHydrax components and adds MHydrax to the scene.
		/// </summary>
		/// <remarks>Call when all params are set.</remarks>
		void Create() {
			if(!disposed) {
				internalHydrax->create();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
        /// Removes MHydrax from the scene.
		/// </summary>
		/// <remarks>
		/// You can call this method to remove MHydrax from the scene
		/// or release (secondary) MHydrax memory, call Create() to return MHydrax to the scene.
		/// </remarks>
		void Remove() {
			if(!disposed) {
				internalHydrax->remove();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
        /// Call this every frame to update the scene.
		/// </summary>
		/// <param name="timeSinceLastFrame">Time since last frame.</param>
		void Update(float timeSinceLastFrame) {
			if(!disposed) {
				internalHydrax->update(timeSinceLastFrame);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
        /// Returns if the specified component is active.
		/// </summary>
		/// <param name="component">Component to be checked.</param>
		bool IsComponent(MHydraxComponent component) {
			if(!disposed) {
				return internalHydrax->isComponent((Hydrax::HydraxComponent)component);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
        /// Sets MHydrax Module.
		/// </summary>
		/// <param name="module">MHydrax Module.</param>
		/// <remarks>Module must be set before calling Create().</remarks>
		void SetModule(MHydrax::MModule^ module) {
			SetModule(module, true);
		}
		/// <summary>
        /// Sets MHydrax Module.
		/// </summary>
		/// <param name="module">MHydrax module.</param>
		/// <param name="deleteOldModule">Delete, if exists, the old Module.</param>
		/// <remarks>Module must be set before calling Create().</remarks>
		void SetModule(MHydrax::MModule^ module, bool deleteOldModule) {
			if(!disposed) {
				internalHydrax->setModule((Hydrax::Module::Module*)module, deleteOldModule);
				if(!deleteOldModule) {
					if(mModule != nullptr) {
						mModule->OwnsNativeRef = true;
					}
				}
				mModule = module;
				mModule->OwnsNativeRef = false;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Set polygon mode (Solid, Wireframe, Points).
		/// </summary>
		/// <param name="pm">Polygon mode.</param>
		void SetPolygonMode(Mogre::PolygonMode pm) {
			if(!disposed) {
				internalHydrax->setPolygonMode((Ogre::PolygonMode&)pm);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
        /// Gets or sets the shader mode.
		/// </summary>
		/// <param name="value">Shader mode.</param>
		/// <returns>Current shader mode.</returns>
		property MMaterialManager::MShaderMode ShaderMode {
			MMaterialManager::MShaderMode get() {
				if(!disposed) {
					return (MMaterialManager::MShaderMode)internalHydrax->getShaderMode();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(MMaterialManager::MShaderMode value) {
				if(!disposed) {
					internalHydrax->setShaderMode((Hydrax::MaterialManager::ShaderMode)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
        /// Gets or sets the water position.
		/// </summary>
		/// <param name="value">Water position.</param>
		/// <returns>Water position.</returns>
		property Mogre::Vector3 Position {
			Mogre::Vector3 get() {
				if(!disposed) {
					return GetManagedVector3(internalHydrax->getPosition());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Vector3 value) {
				if(!disposed) {
					internalHydrax->setPosition((Ogre::Vector3&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
        /// Rotates water and planes.
		/// </summary>
		/// <param name="q">Mogre.Quaternion.</param>
		void Rotate(Mogre::Quaternion q) {
			if(!disposed) {
				internalHydrax->rotate((Ogre::Quaternion&)q);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
        /// Saves MHydrax config to file.
		/// </summary>
		/// <param name="file">File name.</param>
		/// <param name="path">File path.</param>
		/// <returns>False, if an error has ocurred (Check the log file in this case).</returns>
		/// <remarks>If Module isn't set, Module/Noise options won't be saved.</remarks>
		bool SaveCfg(System::String^ file, System::String^ path) {
			if(!disposed) {
				return internalHydrax->saveCfg(GetUnmanagedString(file), GetUnmanagedString(path));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}
		/// <summary>
        /// Saves MHydrax config to file.
		/// </summary>
		/// <param name="file">File name.</param>
		/// <returns>False, if an error has ocurred (Check the log file in this case).</returns>
		/// <remarks>If Module isn't set, Module/Noise options won't be saved.</remarks>
		bool SaveCfg(System::String^ file) {
			return SaveCfg(file, "");
		}

		/// <summary>
        /// Loads MHydrax config from file.
		/// </summary>
		/// <param name="file">File name.</param>
		/// <returns>False, if an error has ocurred (Check the log file in this case).</returns>
		/// <remarks>
		/// The file must be registered in Hydrax resource group.
		/// If Module isn't set, or Module isn't the same from config file, Module options won't be loaded.
		/// </remarks>
		bool LoadCfg(System::String^ file) {
			if(!disposed) {
				return internalHydrax->loadCfg(GetUnmanagedString(file));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
        /// Gets or sets clip planes error.
		/// </summary>
		/// <param name="value">Clip planes error.</param>
		/// <returns>Current clip planes error.</returns>
		property float PlanesError {
			float get() {
				if(!disposed) {
					return internalHydrax->getPlanesError();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setPlanesError(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
        /// Gets or sets the sun position.
		/// </summary>
		/// <param name="value">Sun position.</param>
		/// <returns>Sun position.</returns>
		property Mogre::Vector3 SunPosition {
			Mogre::Vector3 get() {
				if(!disposed) {
					return GetManagedVector3(internalHydrax->getSunPosition());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Vector3 value) {
				if(!disposed) {
					internalHydrax->setSunPosition((Ogre::Vector3&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
        /// Gets or sets the sun strength.
		/// </summary>
		/// <param name="value">Sun strength.</param>
		/// <returns>Sun strength.</returns>
		property float SunStrength {
			float get() {
				if(!disposed) {
					return internalHydrax->getSunStrength();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setSunStrength(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
        /// Gets or sets the sun area.
		/// </summary>
		/// <param name="value">Sun area.</param>
		/// <returns>Sun area.</returns>
		property float SunArea {
			float get() {
				if(!disposed) {
					return internalHydrax->getSunArea();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setSunArea(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
        /// Gets or sets the sun color.
		/// </summary>
		/// <param name="value">Sun color.</param>
		/// <returns>Sun color.</returns>
		property Mogre::Vector3 SunColor {
			Mogre::Vector3 get() {
				if(!disposed) {
					return GetManagedVector3(internalHydrax->getSunColor());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Vector3 value) {
				if(!disposed) {
					internalHydrax->setSunColor((Ogre::Vector3&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the full reflection distance.
		/// </summary>
		/// <param name="value">Water full reflection distance.</param>
		/// <returns>Water full reflection distance.</returns>
		property float FullReflectionDistance {
			float get() {
				if(!disposed) {
					return internalHydrax->getFullReflectionDistance();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setFullReflectionDistance(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the global transparency distance.
		/// </summary>
		/// <param name="value">Global transparency distance.</param>
		/// <returns>Global transparency distance.</returns>
		property float GlobalTransparency {
			float get() {
				if(!disposed) {
					return internalHydrax->getGlobalTransparency();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setGlobalTransparency(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
        /// Gets or sets the water color.
		/// </summary>
		/// <param name="value">Water color.</param>
		/// <returns>Water color.</returns>
		property Mogre::Vector3 WaterColor {
			Mogre::Vector3 get() {
				if(!disposed) {
					return GetManagedVector3(internalHydrax->getWaterColor());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Vector3 value) {
				if(!disposed) {
					internalHydrax->setWaterColor((Ogre::Vector3&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the normal distortion.
		/// </summary>
		/// <param name="value">Normal distortion.</param>
		/// <returns>Normal distortion.</returns>
		/// <remarks>Value should be very short, like 0.025.</remarks>
		property float NormalDistortion {
			float get() {
				if(!disposed) {
					return internalHydrax->getNormalDistortion();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setNormalDistortion(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the foam max distance.
		/// </summary>
		/// <param name="value">Foam max distance.</param>
		/// <returns>Foam max distance.</returns>
		property float FoamMaxDistance {
			float get() {
				if(!disposed) {
					return internalHydrax->getFoamMaxDistance();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setFoamMaxDistance(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the foam scale.
		/// </summary>
		/// <param name="value">Foam scale.</param>
		/// <returns>Foam scale.</returns>
		property float FoamScale {
			float get() {
				if(!disposed) {
					return internalHydrax->getFoamScale();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setFoamScale(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the foam start.
		/// </summary>
		/// <param name="value">Foam start.</param>
		/// <returns>Foam start.</returns>
		property float FoamStart {
			float get() {
				if(!disposed) {
					return internalHydrax->getFoamStart();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setFoamStart(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the foam transparency.
		/// </summary>
		/// <param name="value">Foam transparency.</param>
		/// <returns>Foam transparency.</returns>
		property float FoamTransparency {
			float get() {
				if(!disposed) {
					return internalHydrax->getFoamTransparency();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setFoamTransparency(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the depth limit.
		/// </summary>
		/// <param name="value">Depth limit.</param>
		/// <returns>Depth limit.</returns>
		property float DepthLimit {
			float get() {
				if(!disposed) {
					return internalHydrax->getDepthLimit();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setDepthLimit(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the smooth power.
		/// </summary>
		/// <param name="value">Smooth power.</param>
		/// <returns>Smooth power.</returns>
		/// <remarks>Less values equals more transition distance, high values short transition values, 1-50 range(aprox.).</remarks>
		property float SmoothPower {
			float get() {
				if(!disposed) {
					return internalHydrax->getSmoothPower();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setSmoothPower(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the caustics scale.
		/// </summary>
		/// <param name="value">Caustics scale.</param>
		/// <returns>Caustics scale.</returns>
		property float CausticsScale {
			float get() {
				if(!disposed) {
					return internalHydrax->getCausticsScale();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setCausticsScale(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the custics power.
		/// </summary>
		/// <param name="value">Caustics power.</param>
		/// <returns>Caustics power.</returns>
		property float CausticsPower {
			float get() {
				if(!disposed) {
					return internalHydrax->getCausticsPower();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setCausticsPower(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the caustics end.
		/// </summary>
		/// <param name="value">Caustics end.</param>
		/// <returns>Caustics end.</returns>
		property float CausticsEnd {
			float get() {
				if(!disposed) {
					return internalHydrax->getCausticsEnd();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setCausticsEnd(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the god rays exposure.
		/// </summary>
		/// <param name="value">God rays exposure factors.</param>
		/// <returns>God rays exposure factors.</returns>
		property Mogre::Vector3 GodRaysExposure {
			Mogre::Vector3 get() {
				if(!disposed) {
					return GetManagedVector3(internalHydrax->getGodRaysExposure());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Vector3 value) {
				if(!disposed) {
					internalHydrax->setGodRaysExposure((Ogre::Vector3&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the god rays intensity.
		/// </summary>
		/// <param name="value">God rays intensity.</param>
		/// <returns>God rays intensity.</returns>
		property float GodRaysIntensity {
			float get() {
				if(!disposed) {
					return internalHydrax->getGodRaysIntensity();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setGodRaysIntensity(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Shows/hides hydrax water.
		/// </summary>
		/// <param name="value">True to show, false to hide.</param>
		/// <returns>True, if hydrax water is visible. Otherwise false.</returns>
		/// <remarks>Resources aren't going to be realeased (use Remove() for this), only RTTs are going to be stopped.</remarks>
		property bool Visible {
			bool get() {
				if(!disposed) {
					return internalHydrax->isVisible();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(bool value) {
				if(!disposed) {
					internalHydrax->setVisible(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Determines if Create() has been called already.
		/// </summary>
		/// <returns>True, if Create() has been called already. Otherwise false.</returns>
		property bool IsCreated {
			bool get() {
				if(!disposed) {
					return internalHydrax->isCreated();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the y-displacement under the water needed to change between underwater and overwater mode.
		/// </summary>
		/// <param name="value">Underwater camera switch delta factor.</param>
		/// <returns>Underwater camera switch delta factor.</returns>
		/// <remarks>Useful to get a nice underwater-overwater transition, it depends of the world scale.</remarks>
		property float UnderwaterCameraSwitchDelta {
			float get() {
				if(!disposed) {
					return internalHydrax->getUnderwaterCameraSwitchDelta();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalHydrax->setUnderwaterCameraSwitchDelta(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the rendering camera.
		/// </summary>
		/// <returns>The camera assigned to this MHydrax object.</returns>
		property Mogre::Camera^ Camera {
			Mogre::Camera^ get() {
				if(!disposed) {
					return (Mogre::Camera^)internalHydrax->getCamera();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the main window viewport.
		/// </summary>
		/// <returns>The viewport assigned to this MHydrax object.</returns>
		property Mogre::Viewport^ Viewport {
			Mogre::Viewport^ get() {
				if(!disposed) {
					return (Mogre::Viewport^)internalHydrax->getViewport();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the scene manager.
		/// </summary>
		/// <returns>The scene manager assigned to this MHydrax object.</returns>
		property Mogre::SceneManager^ SceneManager {
			Mogre::SceneManager^ get() {
				if(!disposed) {
					return (Mogre::SceneManager^)internalHydrax->getSceneManager();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the MHydrax::MMesh.
		/// </summary>
		/// <returns>MHydrax::MMesh.</returns>
		property MMesh^ Mesh {
			MMesh^ get() {
				if(!disposed) {
					return gcnew MMesh(internalHydrax->getMesh());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the associated MaterialManager.
		/// </summary>
		/// <returns>The associated MaterialManager.</returns>
		property MMaterialManager^ MaterialManager {
			MMaterialManager^ get() {
				if(!disposed) {
					return gcnew MMaterialManager(internalHydrax->getMaterialManager());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		///** Get Hydrax::RttManager
		//    @return Hydrax::RttManager pointer
		// */
		//inline RttManager* getRttManager()
		//{
		//	return mRttManager;
		//}

		///** Get Hydrax::TextureManager
		//    @return Hydrax::TextureManager pointer
		// */
		//inline TextureManager* getTextureManager()
		//{
		//	return mTextureManager;
		//}

		/// <summary>
		/// Gets the associated GodRaysManager.
		/// </summary>
		/// <returns>The associated GodRaysManager.</returns>
		property MGodRaysManager^ GodRaysManager {
			MGodRaysManager^ get() {
				if(!disposed) {
					return gcnew MGodRaysManager(internalHydrax->getGodRaysManager());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the associated DecalsManager.
		/// </summary>
		/// <returns>The associated DecalsManager.</returns>
		property MDecalsManager^ DecalsManager {
			MDecalsManager^ get() {
				if(!disposed) {
					return gcnew MDecalsManager(internalHydrax->getDecalsManager());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		///** Get Hydrax::GPUNormalMapManager
		//    @return Hydrax::GPUNormalMapManager pointer
		//    */
		//inline GPUNormalMapManager* getGPUNormalMapManager()
		//{
		//	return mGPUNormalMapManager;
		//}

		/// <summary>
		/// Gets the associated CfgFileManager.
		/// </summary>
		/// <returns>The associated CfgFileManager.</returns>
		property MCfgFileManager^ CfgFileManager {
			MCfgFileManager^ get() {
				if(!disposed) {
					return gcnew MCfgFileManager(internalHydrax->getCfgFileManager());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the assigned Module.
		/// </summary>
		/// <returns>The assigned Module or NULL if Module isn't set.</returns>
		property MModule^ Module {
			MModule^ get() {
				if(!disposed) {
					return mModule;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the current hydrax components.
		/// </summary>
		/// <param name="value">Components.</param>
		/// <returns>The current components.</returns>
		/// <remarks>It can be called after Create(), Components will be updated.</remarks>
		property MHydraxComponent Components {
			MHydraxComponent get() {
				if(!disposed) {
					return (MHydraxComponent)internalHydrax->getComponents();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(MHydraxComponent value) {
				if(!disposed) {
					return internalHydrax->setComponents((Hydrax::HydraxComponent&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}		
		}

		/// <summary>
		/// Gets the current polygon mode.
		/// </summary>
		/// <returns>The current polygon mode.</returns>
		property Mogre::PolygonMode PolygonMode {
			Mogre::PolygonMode get() {
				if(!disposed) {
					return (Mogre::PolygonMode)internalHydrax->getPolygonMode();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the current height at a specified world-space point.
		/// </summary>
		/// <param name="position">X/Z World position.</param>
		/// <returns>The Height at the given position in y-World coordinates. Returns -1, if it's outside of the water.</returns>
		float GetHeight(Mogre::Vector2 position) {
			if(!disposed) {
				return internalHydrax->getHeigth((Ogre::Vector2&)position);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets the current height at a specified world-space point.
		/// </summary>
		/// <param name="position">X/(Y)/Z World position.</param>
		/// <returns>The Height at the given position in y-World coordinates. Returns -1, if it's outside of the water.</returns>
		float GetHeight(Mogre::Vector3 position) {
			if(!disposed) {
				return internalHydrax->getHeigth((Ogre::Vector3&)position);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Determines if current frame is underwater.
		/// </summary>
		/// <returns>True, if yes. Otherwise false.</returns>
		property bool IsCurrentFrameUnderwater {
			bool get() {
				if(!disposed) {
					return internalHydrax->_isCurrentFrameUnderwater();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MHydrax() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources
				if(mModule != nullptr) {
					delete mModule;
				}

				this->!MHydrax();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MHydrax() {
			// dispose unmanaged resources
			delete internalHydrax;

			disposed = true;
		}

	};
}