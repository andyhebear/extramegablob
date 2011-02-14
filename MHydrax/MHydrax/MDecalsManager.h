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

// MDecalsManager.h

#pragma once

namespace MHydrax {

	/// <summary>
	/// Decal class.
	/// </summary>
	public ref class MDecal : System::IDisposable {
	private:
		Hydrax::Decal* internalDecal;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MDecal.
		/// </summary>
		///	<param name="nativeDecal">A native Hydrax::Decal.</param>
		MDecal(Hydrax::Decal* nativeDecal) {
			internalDecal = nativeDecal;
			ownsNativeRef = false;
		}

	public:
		/// <summary>
		/// Conversion operator to type Hydrax::Decal*.
		/// </summary>
		operator Hydrax::Decal *() {
			return internalDecal;
		}

		/// <summary>
		/// Register the decal int the specified pass.
		/// </summary>
		/// <param name="pass">Pass to be registred.</param>
		void RegisterPass(Mogre::Pass^ pass) {
			if(!disposed) {
				internalDecal->registerPass((Ogre::Pass*)pass);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Unregister from current technique.
		/// </summary>
		void Unregister() {
			if(!disposed) {
				internalDecal->unregister();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets or sets the decal texture (file)name.
		/// </summary>
		/// <param name="value">Decal texture name.</param>
		/// <returns>Decal texture name.</returns>
		property System::String^ TextureName {
			System::String^ get() {
				if(!disposed) {
					return GetManagedString(internalDecal->getTextureName());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the decal Id.
		/// </summary>
		/// <returns>Decal Id.</returns>
		property int Id {
			int get() {
				if(!disposed) {
					return internalDecal->getId();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the decal projector.
		/// </summary>
		/// <returns>Projector frustum.</returns>
		property Mogre::Frustum^ Projector {
			Mogre::Frustum^ get() {
				if(!disposed) {
					return (Mogre::Frustum^)internalDecal->getProjector();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the decal scene node.
		/// </summary>
		/// <returns>Decal scene node.</returns>
		property Mogre::SceneNode^ SceneNode {
			Mogre::SceneNode^ get() {
				if(!disposed) {
					return (Mogre::SceneNode^)internalDecal->getSceneNode();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the pass the decal is in.
		/// </summary>
		/// <returns>Registered pass.</returns>
		/// <remarks>Returns NULL if decal isn't registered.</remarks>
		property Ogre::Pass* RegisteredPass {
			Ogre::Pass* get() {
				if(!disposed) {
					return internalDecal->getRegisteredPass();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the decal position.
		/// </summary>
		/// <param name="value">Decal position.</param>
		/// <returns>Decal position.</returns>
		property Mogre::Vector2 Position {
			Mogre::Vector2 get() {
				if(!disposed) {
					return GetManagedVector2(internalDecal->getPosition());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Vector2 value) {
				if(!disposed) {
					return internalDecal->setPosition((Ogre::Vector2&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the decal size (in world coordinates).
		/// </summary>
		/// <param name="value">Decal size in world coordinates.</param>
		/// <returns>Decal size.</returns>
		property Mogre::Vector2 Size {
			Mogre::Vector2 get() {
				if(!disposed) {
					return GetManagedVector2(internalDecal->getSize());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Vector2 value) {
				if(!disposed) {
					return internalDecal->setSize((Ogre::Vector2&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the decal orientation.
		/// </summary>
		/// <param name="value">Decal orientation.</param>
		/// <returns>Decal orientation.</returns>
		property Mogre::Radian Orientation {
			Mogre::Radian get() {
				if(!disposed) {
					return (Mogre::Radian)internalDecal->getOrientation();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(Mogre::Radian value) {
				if(!disposed) {
					return internalDecal->setOrientation((Ogre::Radian&)value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the decal transparency.
		/// </summary>
		/// <param name="value">Decal transparency in [0,1] range.</param>
		/// <returns>Decal transparency.</returns>
		/// <remarks>0 = Full transparent, 1 = Full opacity.</remarks>
		property float Transparency {
			float get() {
				if(!disposed) {
					return internalDecal->getTransparency();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(float value) {
				if(!disposed) {
					return internalDecal->setTransparency(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if decal is visible.
		/// </summary>
		/// <param name="value">Decal visibility.</param>
		/// <returns>True if decal is visible. Otherwise false.</returns>
		property bool IsVisible {
			bool get() {
				if(!disposed) {
					return internalDecal->isVisible();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(bool value) {
				if(!disposed) {
					return internalDecal->setVisible(value);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MDecal() { // implements IDisposable
			// dispose managed resources

			this->!MDecal();
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MDecal() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalDecal;
			}

			disposed = true;
		}
	};

	/// <summary>
	/// Decals manager class. Use it for place any kind of texture
	/// over the water! Like ship trails, overwater vegetables, ...
	/// </summary>
	public ref class MDecalsManager : System::IDisposable {
	private:
		Hydrax::DecalsManager* internalDecalsManager;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MDecalsManager.
		/// </summary>
		///	<param name="nativeDecalsManager">A native Hydrax::DecalsManager.</param>
		MDecalsManager(Hydrax::DecalsManager* nativeDecalsManager) {
			internalDecalsManager = nativeDecalsManager;
			ownsNativeRef = false;
		}

	public:
		/// <summary>
		/// Conversion operator to type Hydrax::DecalsManager*.
		/// </summary>
		operator Hydrax::DecalsManager *() {
			if(!disposed) {
				return internalDecalsManager;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Update decal manager.
		/// </summary>
		/// <remarks>Call each frame.</remarks>
		void Update() {
			if(!disposed) {
				internalDecalsManager->update();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}
		
		/// <summary>
		/// Add decal.
		/// </summary>
		/// <param name="textureName">Texture name.</param>
		/// <returns>A MHydrax::MDecal^ Use it as a usual Ogre::SceneNode(Decal::getSceneNode()) for position, rotate...etc!</returns>
		MDecal^ Add(System::String^ textureName) {
			if(!disposed) {
				return gcnew MDecal(internalDecalsManager->add(GetUnmanagedString(textureName)));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}
		
		/// <summary>
		/// Get decal.
		/// </summary>
		/// <param name="id">Decal Id.</param>
		/// <returns>A MHydrax::MDecal^</returns>
		MDecal^ Get(int id) {
			if(!disposed) {
				return gcnew MDecal(internalDecalsManager->get(id));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove decal.
		/// </summary>
		/// <param name="id">Decal Id.</param>
		void Remove(int id) {
			if(!disposed) {
				internalDecalsManager->remove(id);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove all decals.
		/// </summary>
		void RemoveAll() {
			if(!disposed) {
				internalDecalsManager->removeAll();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Register all decals.
		/// </summary>
		/// <remarks>Use it when water material is (re)created.</remarks>
		void RegisterAll() {
			if(!disposed) {
				internalDecalsManager->registerAll();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets list of MDecals.
		/// </summary>
		/// <returns>A generic list of MDecals.</returns>
		property System::Collections::Generic::List<MDecal^>^ Decals {
			System::Collections::Generic::List<MDecal^>^ get() {
				if(!disposed) {
					std::vector<Hydrax::Decal*> list = internalDecalsManager->getDecals();
					System::Collections::Generic::List<MDecal^>^ l = gcnew System::Collections::Generic::List<MDecal^>;
					for(unsigned int i=0; i < list.size(); i++) {
						l->Add(gcnew MDecal(list[i]));
					}
					return l;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MDecalsManager() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources

				this->!MDecalsManager();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MDecalsManager() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalDecalsManager;
			}

			disposed = true;
		}

	};
}
