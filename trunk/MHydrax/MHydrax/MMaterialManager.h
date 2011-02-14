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

// MMaterialManager.h

#pragma once

namespace MHydrax {

	/// <summary>
	/// Material/Shader manager class.
	/// </summary>
	public ref class MMaterialManager : System::IDisposable {
	private:
		Hydrax::MaterialManager* internalMaterialManager;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MMaterialManager.
		/// </summary>
		///	<param name="nativeMaterialManager">A native Hydrax::MaterialManager.</param>
		MMaterialManager(Hydrax::MaterialManager* nativeMaterialManager) {
			internalMaterialManager = nativeMaterialManager;
			ownsNativeRef = false;
		}

	public:
		/// <summary>
		/// Material type enum.
		/// </summary>
		/// <remarks>Use in GetMaterial(MaterialType).</remarks>
		enum class MMaterialType {
			// Water material
			MAT_WATER = 0,
			// Depth material
			MAT_DEPTH = 1,
			// Underwater material
			MAT_UNDERWATER = 2,
			// Compositor material(material wich is used in underwater compositor)
			MAT_UNDERWATER_COMPOSITOR = 3,
			// Simple red material
			MAT_SIMPLE_RED = 4,
			// Simple black material
			MAT_SIMPLE_BLACK = 5
		};

		/// <summary>
		/// Compositor type enum.
		/// </summary>
		/// <remarks>Use in GetCompositor(CompositorType).</remarks>
		enum class MCompositorType {
			// Underwater compositor
			COMP_UNDERWATER = 0
		};

		/// <summary>
		/// Gpu program enum.
		/// </summary>
		/// <remarks>Use in SetGpuProgramParameter().</remarks>
		enum class MGpuProgram {
			// Vertex program
			GPUP_VERTEX   = 0,
			// Fragment program
			GPUP_FRAGMENT = 1
		};

		/// <summary>
		/// Normal generation mode.
		/// </summary>
		enum class MNormalMode {
			// Normal map from precomputed texture(CPU)
			NM_TEXTURE,
			// Normal map from vertex(CPU)
			NM_VERTEX,
			// Normal map from RTT(GPU)
			NM_RTT
		};

		/// <summary>
		/// Shader mode.
		/// </summary>
		enum class MShaderMode {
			// HLSL
			SM_HLSL,
			// Cg
			SM_CG,
			// GLSL
			SM_GLSL
		};

		/// <summary>
		/// Material options.
		/// </summary>
		ref class MOptions {
		private:
			Hydrax::MaterialManager::Options* internalOptions;

			// Track whether Dispose has been called.
			bool disposed;

		internal:
			/// <summary>
			/// Creates a new instance of MMaterialManager.MOptions.
			/// </summary>
			///	<param name="nativeOptions">A native Hydrax::MaterialManager::Options.</param>
			MOptions(Hydrax::MaterialManager::Options* nativeOptions) {
				internalOptions = nativeOptions;
			}

		public:
			/// <summary>
			/// Conversion operator to type Hydrax::MaterialManager::Options*.
			/// </summary>
			operator Hydrax::MaterialManager::Options *() {
				if(!disposed) {
					return internalOptions;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Shader mode.
			/// </summary>
			property MShaderMode ShaderMode {
				MShaderMode get() {
					if(!disposed) {
						return (MShaderMode)internalOptions->SM;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Normal map generation mode.
			/// </summary>
			property MNormalMode NormalMode {
				MNormalMode get() {
					if(!disposed) {
						return (MNormalMode)internalOptions->NM;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}
		};

		/// <summary>
		/// Conversion operator to type Hydrax::MaterialManager*.
		/// </summary>
		operator Hydrax::MaterialManager *() {
			if(!disposed) {
				return internalMaterialManager;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Create materials.
		/// </summary>
		/// <param name="components">Components of the shader.</param>
		/// <param name="components">Material options.</param>
		bool CreateMaterials(MHydraxComponent components, MOptions^ options) {
			if(!disposed) {
				return internalMaterialManager->createMaterials((Hydrax::HydraxComponent)components, (Hydrax::MaterialManager::Options&)options);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove materials.
		/// </summary>
		/// <remarks>RemoveCompositor() is called too.</remarks>
		void RemoveMaterials() {
			if(!disposed) {
				internalMaterialManager->removeMaterials();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove compositor.
		/// </summary>
		void RemoveCompositor() {
			if(!disposed) {
				internalMaterialManager->removeCompositor();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Reload material.
		/// </summary>
		/// <param name="material">Material to reload.</param>
		void Reload(MMaterialType material) {
			if(!disposed) {
				internalMaterialManager->reload((Hydrax::MaterialManager::MaterialType&)material);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		///** Fill GPU vertex and fragment program to a pass
		//    @param Pass Pass to fill Gpu programs
		//	@param GpuProgramNames [0]: Vertex program name, [1]: Fragment program name
		//	@param SM Shader mode, note: Provided data strings will correspong with selected shader mode
		//	@param EntryPoints [0]: Vertex program entry point, [1]: Fragment program entry point
		//	@param Data [0] Vertex program data, [1]: Fragment program data
		// */
		//bool fillGpuProgramsToPass(Ogre::Pass* Pass,
		//					       const Ogre::String GpuProgramNames[2],
		//					       const ShaderMode& SM,
		//					       const Ogre::String EntryPoints[2], 
		//					       const Ogre::String Data[2]);

		///** Create GPU program
		//	@param Name HighLevelGpuProgram name
		//	@param SM Shader mode
		//	@param GPUP GpuProgram type
		//	@param EntryPoint Entry point
		//	@param Data
		// */
		//bool createGpuProgram(const Ogre::String &Name,
		//	                  const ShaderMode& SM, 
		//					  const GpuProgram& GPUP, 
		//					  const Ogre::String& EntryPoint, 
		//					  const Ogre::String& Data);

		/// <summary>
		/// Gets a value indicating if createMaterials() has been called already.
		/// </summary>
		/// <returns>True, if createMaterials() has been called. Otherwise False.</returns>
		property bool IsCreated {
			bool get() {
				if(!disposed) {
					return internalMaterialManager->isCreated();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets material.
		/// </summary>
		/// <param name="material">Material to get.</param>
		/// <returns>MaterialPtr.</returns>
		Mogre::MaterialPtr^ GetMaterial(MMaterialType material) {
			if(!disposed) {
				return (Mogre::MaterialPtr^)internalMaterialManager->getMaterial((Hydrax::MaterialManager::MaterialType)material);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets compositor.
		/// </summary>
		/// <param name="compositor">Compositor to get.</param>
		/// <returns>CompositorPtr.</returns>
		Mogre::CompositorPtr^ GetCompositor(MCompositorType compositor) {
			if(!disposed) {
				return (Mogre::CompositorPtr^)internalMaterialManager->getCompositor((Hydrax::MaterialManager::CompositorType)compositor);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets a value indicating if the compositor is enabled.
		/// </summary>
		/// <param name="compositor">Compositor to check.</param>
		/// <returns>True if enabled. Otherwise False.</returns>
		bool IsCompositorEnabled(MCompositorType compositor) {
			if(!disposed) {
				return internalMaterialManager->isCompositorEnable((Hydrax::MaterialManager::CompositorType)compositor);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Set a compositor enabled/disabled.
		/// </summary>
		/// <param name="compositor">Compositor to change.</param>
		/// <param name="enable">True to enable, False to disable.</param>
		void SetCompositorEnabled(MCompositorType compositor, bool enable) {
			if(!disposed) {
				internalMaterialManager->setCompositorEnable((Hydrax::MaterialManager::CompositorType)compositor, enable);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets the last MMaterialManager::MOptions used in a material generation.
		/// </summary>
		/// <returns>Last MMaterialManager::MOptions used in a material generation.</returns>
		property MOptions^ LastOptions {
			MOptions^ get() {
				if(!disposed) {
					return gcnew MOptions((Hydrax::MaterialManager::Options*)&internalMaterialManager->getLastOptions());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Add depth technique to a specified material.
		/// </summary>
		/// <param name="technique">Technique where depth technique will be added.</param>
		/// <param name="autoUpdate">The technique will be automatically updated when water parameters change.</param>
		/// <remarks>
		/// Call it after MHydrax.Create()/MHydrax.SetComponents(...)
		/// The technique will be automatically updated when water parameters change if parameter AutoUpdate == true
		/// Add depth technique when a material is not an Ogre::Entity, such terrains, PLSM2 materials, etc.
		/// This depth technique will be added with "HydraxDepth" scheme in ordeto can use it in the Depth RTT. 
		/// </remarks>
		void AddDepthTechnique(Mogre::Technique^ technique, bool autoUpdate) {
			if(!disposed) {
				internalMaterialManager->addDepthTechnique((Ogre::Technique*)technique, autoUpdate);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}
		/// <summary>
		/// Add depth technique to a specified material.
		/// </summary>
		/// <param name="technique">Technique where depth technique will be added.</param>
		/// <remarks>
		/// Call it after MHydrax.Create()/MHydrax.SetComponents(...)
		/// Add depth technique when a material is not an Ogre::Entity, such terrains, PLSM2 materials, etc.
		/// This depth technique will be added with "HydraxDepth" scheme in ordeto can use it in the Depth RTT. 
		/// </remarks>
		void AddDepthTechnique(Mogre::Technique^ technique) {
			AddDepthTechnique(technique, true);
		}

		/// <summary>
		/// Gets external depth techniques.
		/// </summary>
		/// <returns>A generic list of Mogre::Techniques.</returns>
		property System::Collections::Generic::List<Mogre::Technique^>^ DepthTechniques {
			System::Collections::Generic::List<Mogre::Technique^>^ get() {
				if(!disposed) {
					std::vector<Ogre::Technique*> list = internalMaterialManager->getDepthTechniques();
					System::Collections::Generic::List<Mogre::Technique^>^ l = gcnew System::Collections::Generic::List<Mogre::Technique^>;
					for(unsigned int i=0; i < list.size(); i++) {
						l->Add((Mogre::Technique^)list[i]);
					}
					return l;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		///** Set gpu program Ogre::Real parameter
		//    @param GpuP Gpu program type (Vertex/Fragment)
		//	@param MType Water/Depth material
		//	@param Name param name
		//	@param Value value
		// */
		//void setGpuProgramParameter(const GpuProgram &GpuP, const MaterialType &MType, const Ogre::String &Name, const Ogre::Real &Value);

		///** Set gpu program Ogre::Vector2 parameter
		//    @param GpuP Gpu program type (Vertex/Fragment)
		//	@param MType Water/Depth material
		//	@param Name param name
		//	@param Value value
		// */
		//void setGpuProgramParameter(const GpuProgram &GpuP, const MaterialType &MType, const Ogre::String &Name, const Ogre::Vector2 &Value); 

		///** Set gpu program Ogre::Vector3 parameter
		//    @param GpuP Gpu program type (Vertex/Fragment)
		//	@param MType Water/Depth material
		//	@param Name param name
		//	@param Value value
		// */
		//void setGpuProgramParameter(const GpuProgram &GpuP, const MaterialType &MType, const Ogre::String &Name, const Ogre::Vector3 &Value); 

		/// <summary>
		/// Destructor.
		/// </summary>
		~MMaterialManager() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources

				this->!MMaterialManager();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MMaterialManager() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalMaterialManager;
			}

			disposed = true;
		}

	};
}
