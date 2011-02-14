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

// MGodRaysManager.h

#pragma once

namespace MHydrax {

	/// <summary>
	/// Underwater god rays manager class.
	/// </summary>
	public ref class MGodRaysManager : System::IDisposable {
	private:
		Hydrax::GodRaysManager* internalGodRaysManager;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MGodRaysManager.
		/// </summary>
		///	<param name="nativeGodRaysManager">A native Hydrax::GodRaysManager.</param>
		MGodRaysManager(Hydrax::GodRaysManager* nativeGodRaysManager) {
			internalGodRaysManager = nativeGodRaysManager;
			ownsNativeRef = false;
		}

	public:
		/// <summary>
		/// Conversion operator to type Hydrax::GodRaysManager*.
		/// </summary>
		operator Hydrax::GodRaysManager *() {
			if(!disposed) {
				return internalGodRaysManager;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="components">Current Hydrax components.</param>
		void Create(MHydraxComponent components) {
			if(!disposed) {
				internalGodRaysManager->create((Hydrax::HydraxComponent)components);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove.
		/// </summary>
		void Remove(MHydraxComponent components) {
			if(!disposed) {
				internalGodRaysManager->remove();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Update. Call each frame.
		/// </summary>
		/// <param name="timeSinceLastFrame">Time since last frame.</param>
		void Update(float timeSinceLastFrame) {
			if(!disposed) {
				internalGodRaysManager->update(timeSinceLastFrame);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Determines if Create() has been called already.
		/// </summary>
		/// <returns>True, if Create() has been called already. Otherwise false.</returns>
		property bool IsCreated {
			bool get() {
				if(!disposed) {
					return internalGodRaysManager->isCreated();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Add god rays depth technique to a specified material.
		/// </summary>
		/// <param name="technique">Technique where depth technique will be added.</param>
		/// <param name="autoUpdate">The technique will be automatically updated when god rays parameters change.</param>
		/// <remarks>
		/// Call it after MHydrax.Create()/MHydrax.SetComponents(...)
		/// The technique will be automatically updated when god rays parameters change if parameter AutoUpdate == true
		/// Add depth technique when a material is not an Ogre::Entity, such as terrains, PLSM2 materials, etc.
		/// This depth technique will be added with "HydraxGodRaysDepth" scheme in ordeto can use it in the God Rays depth RTT. 
		/// </remarks>
		void AddDepthTechnique(Mogre::Technique^ technique, bool autoUpdate) {
			if(!disposed) {
				internalGodRaysManager->addDepthTechnique((Ogre::Technique*)technique, autoUpdate);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}
		/// <summary>
		/// Add god rays depth technique to a specified material.
		/// </summary>
		/// <param name="technique">Technique where depth technique will be added.</param>
		/// <remarks>
		/// Call it after MHydrax.Create()/MHydrax.SetComponents(...)
		/// Add depth technique when a material is not an Ogre::Entity, such as terrains, PLSM2 materials, etc.
		/// This depth technique will be added with "HydraxGodRaysDepth" scheme in ordeto can use it in the God Rays Depth RTT. 
		/// </remarks>
		void AddDepthTechnique(Mogre::Technique^ technique) {
			AddDepthTechnique(technique, true);
		}

		/// <summary>
		/// Gets or sets the god rays simulation speed.
		/// </summary>
		/// <param name="value">Simulation speed.</param>
		/// <returns>Simulation speed.</returns>
		property float SimulationSpeed {
			float get() {
				if(!disposed) {
					return internalGodRaysManager->getSimulationSpeed();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalGodRaysManager->setSimulationSpeed(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of god rays.
		/// </summary>
		/// <param name="value">Number of god rays.</param>
		/// <returns>Number of god rays.</returns>
		property int NumberOfRays {
			int get() {
				if(!disposed) {
					return internalGodRaysManager->getNumberOfRays();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(int value) {
				if(!disposed) {
					internalGodRaysManager->setNumberOfRays(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the god rays size.
		/// </summary>
		/// <param name="value">God rays size.</param>
		/// <returns>God rays size.</returns>
		property float RaysSize {
			float get() {
				if(!disposed) {
					return internalGodRaysManager->getRaysSize();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					internalGodRaysManager->setRaysSize(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the perlin noise module.
		/// </summary>
		/// <returns>Perlin noise module.</returns>
		property MPerlin^ Perlin {
			MPerlin^ get() {
				if(!disposed) {
					return gcnew MPerlin(internalGodRaysManager->getPerlin());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the god rays scene node.
		/// </summary>
		/// <returns>God rays scene node.</returns>
		property Mogre::SceneNode^ SceneNode {
			Mogre::SceneNode^ get() {
				if(!disposed) {
					return (Mogre::SceneNode^)internalGodRaysManager->getSceneNode();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if god rays are visible.
		/// </summary>
		/// <param name="value">True to show, false to hide.</param>
		/// <returns>True, if god rays are visible. Otherwise false.</returns>
		property bool Visible {
			bool get() {
				if(!disposed) {
					return internalGodRaysManager->isVisible();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(bool value) {
				if(!disposed) {
					internalGodRaysManager->setVisible(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets god rays objects intersections enabled.
		/// </summary>
		/// <param name="value">True to enable, false to disable.</param>
		/// <returns>True, if god rays objects intersections are enabled. Otherwise false.</returns>
		property bool ObjectsIntersectionsEnabled {
			bool get() {
				if(!disposed) {
					return internalGodRaysManager->areObjectsIntersectionsEnabled();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(bool value) {
				if(!disposed) {
					internalGodRaysManager->setObjectIntersectionsEnabled(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
        /// Gets or sets the noise parameters.
		/// </summary>
		/// <param name="value">Vector4 with the following parameters:
		///					x-> Noise derivation
		///	                y-> Position multiplier
		///					z-> Y normal component multiplier
		///					w-> Normal multiplier</param>
		/// <returns>Vector4 that stores 4 parameters:
		///	                x-> Noise derivation
		///	                y-> Position multiplier
		///					z-> Y normal component multiplier
		///					w-> Normal multiplier</returns>
		property Mogre::Vector4 Position {
			Mogre::Vector4 get() {
				if(!disposed) {
					return GetManagedVector4(internalGodRaysManager->getNoiseParameters());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Vector4 value) {
				if(!disposed) {
					internalGodRaysManager->setNoiseParameters((Ogre::Vector4&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MGodRaysManager() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources

				this->!MGodRaysManager();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MGodRaysManager() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalGodRaysManager;
			}

			disposed = true;
		}

	};
}
