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

// MPerlin.h

#pragma once

#include "MNoise.h"

namespace MHydrax {

	/// <summary>
	/// Perlin noise module class.
	/// </summary>
	public ref class MPerlin : MNoise, System::IDisposable {
	public:
		/// <summary>
		/// Struct wich contains Perlin noise module options.
		/// </summary>
		ref struct MOptions {
		private:
			Hydrax::Noise::Perlin::Options* internalOptions;

			// Track whether this instance owns, or does not own the reference to the native pointer.
			bool ownsNativeRef;

			// Track whether Dispose has been called.
			bool disposed;

		internal:
			/// <summary>
			/// Creates a new instance of MPerlin.MOptions.
			/// </summary>
			/// <param name="nativeOptions">Native Noise::Perlin::Options.</param>
			MOptions(Hydrax::Noise::Perlin::Options* nativeOptions) {
				internalOptions = nativeOptions;
				ownsNativeRef = false;
			}

		public:
			/// <summary>
			/// Creates a new instance of MPerlin.MOptions.
			/// </summary>
			MOptions() {
				internalOptions = new Hydrax::Noise::Perlin::Options();
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MPerlin.MOptions.
			/// </summary>
			/// <param name="octaves">Perlin noise octaves.</param>
			/// <param name="scale">Noise scale.</param>
			/// <param name="falloff">Noise fall off.</param>
			/// <param name="animspeed">Animation speed.</param>
			/// <param name="timemulti">Timemulti.</param>
			MOptions(int octaves,
					float scale,
					float falloff,
					float animspeed,
					float timemulti) {
				internalOptions = new Hydrax::Noise::Perlin::Options(
					octaves, scale, falloff, animspeed, timemulti);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MPerlin.MOptions.
			/// </summary>
			/// <param name="octaves">Perlin noise octaves.</param>
			/// <param name="scale">Noise scale.</param>
			/// <param name="falloff">Noise fall off.</param>
			/// <param name="animspeed">Animation speed.</param>
			/// <param name="timemulti">Timemulti.</param>
			/// <param name="gpu_Strength">GPU_Strength.</param>
			/// <param name="gpu_LodParameters">GPU_LODParameters.</param>
			MOptions(int octaves,
					float scale,
					float falloff,
					float animspeed,
					float timemulti,
					float gpu_Strength,
					Mogre::Vector3 gpu_LodParameters) {
				internalOptions = new Hydrax::Noise::Perlin::Options(
					octaves, scale, falloff, animspeed, timemulti, gpu_Strength, (Ogre::Vector3&)gpu_LodParameters);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Conversion operator to type Hydrax::Noise::Perlin::Options*.
			/// </summary>
			operator Hydrax::Noise::Perlin::Options *() {
				return internalOptions;
			}

			/// <summary>
			/// Conversion operator to type Hydrax::Noise::Perlin::Options.
			/// </summary>
			operator Hydrax::Noise::Perlin::Options(){
				if(!disposed) {
					Hydrax::Noise::Perlin::Options opt = Hydrax::Noise::Perlin::Options(internalOptions->Octaves, internalOptions->Scale, internalOptions->Falloff, internalOptions->Animspeed, internalOptions->Timemulti, internalOptions->GPU_Strength, internalOptions->GPU_LODParameters);
					return opt;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Octaves.
			/// </summary>
			property int Octaves {
				int get() {
					if(!disposed) {
						return internalOptions->Octaves;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(int value) {
					if(!disposed) {
						internalOptions->Octaves = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Scale.
			/// </summary>
			property float Scale {
				float get() {
					if(!disposed) {
						return internalOptions->Scale;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->Scale = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Falloff.
			/// </summary>
			property float Falloff {
				float get() {
					if(!disposed) {
						return internalOptions->Falloff;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->Falloff = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Animspeed.
			/// </summary>
			property float Animspeed {
				float get() {
					if(!disposed) {
						return internalOptions->Animspeed;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->Animspeed = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Timemulti.
			/// </summary>
			property float Timemulti {
				float get() {
					if(!disposed) {
						return internalOptions->Timemulti;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->Timemulti = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Representes the strength of the normals (i.e. Amplitude).
			/// </summary>
			/// <remarks>
			/// GPU Normal map generator parameters. Only if GPU normal map generation is active.
			/// </remarks>
			property float GPU_Strength {
				float get() {
					if(!disposed) {
						return internalOptions->GPU_Strength;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->GPU_Strength = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// LOD Parameters, in order to obtain a smooth normal map we need to 
            /// decrease the detail level when the pixel is far to the camera.
			/// These parameters are stored in a Mogre.Vector3:
			/// x -> Initial LOD value (Bigger values -> less detail)
			///	y -> Final LOD value
			///	z -> Final distance
			/// </summary>
			/// <remarks>
			/// GPU Normal map generator parameters. Only if GPU normal map generation is active.
			/// </remarks>
			property Mogre::Vector3 GPU_LODParameters {
				Mogre::Vector3 get() {
					if(!disposed) {
						return GetManagedVector3(internalOptions->GPU_LODParameters);
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(Mogre::Vector3 value) {
					if(!disposed) {
						internalOptions->GPU_LODParameters = (Ogre::Vector3&)value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Destructor.
			/// </summary>
			~MOptions() { // implements IDisposable
				if(!disposed) {
					// dispose managed resources

					this->!MOptions();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

		protected:
			/// <summary>
			/// Finalizer.
			/// </summary>
			!MOptions() {
				// dispose unmanaged resources
				if(ownsNativeRef) {
					delete internalOptions;
				}

				disposed = true;
			}
		};
	private:
		Hydrax::Noise::Perlin* internalPerlin;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MPerlin.
		/// </summary>
		///	<param name="nativePerlin">A native Hydrax::Noise::Perlin.</param>
		MPerlin(Hydrax::Noise::Perlin* nativePerlin)
			: MNoise() {
			internalPerlin = nativePerlin;
			ownsNativeRef = false;
		}

		virtual property bool OwnsNativeRef {
			bool get() override {
				return ownsNativeRef;
			}
			void set(bool value) override {
				ownsNativeRef = value;
			}
		}

	public:
		/// <summary>
		/// Creates a new instance of MPerlin.
		/// </summary>
		MPerlin()
			: MNoise() {
			internalPerlin = new Hydrax::Noise::Perlin();
			ownsNativeRef = true;
		}

		/// <summary>
		/// Creates a new instance of MPerlin.
		/// </summary>
		/// <param name="options">Options.</param>
		MPerlin(MPerlin::MOptions^ options)
			: MNoise() {
			Hydrax::Noise::Perlin::Options nativeOptions = (Hydrax::Noise::Perlin::Options)*options;
			internalPerlin = new Hydrax::Noise::Perlin(nativeOptions);
			ownsNativeRef = true;
		}

		/// <summary>
		/// Conversion operator to type Hydrax::Noise::Noise*.
		/// </summary>
		virtual operator Hydrax::Noise::Noise *() override {
			if(!disposed) {
				return (Hydrax::Noise::Noise*)internalPerlin;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Conversion operator to type Hydrax::Noise::Perlin*.
		/// </summary>
		operator Hydrax::Noise::Perlin *() {
			if(!disposed) {
				return internalPerlin;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Create.
		/// </summary>
		virtual void Create() override {
			if(!disposed) {
				internalPerlin->create();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove.
		/// </summary>
		virtual void Remove() override {
			if(!disposed) {
				internalPerlin->remove();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/** Create GPUNormalMap resources
		    @param g GPUNormalMapManager pointer
			@return true if it needs to be created, false if not
		 */
		//bool createGPUNormalMapResources(GPUNormalMapManager *g);

		/// <summary>
		/// Call it each frame.
		/// </summary>
		/// <param name="timeSinceLastFrame">Time since last frame(delta).</param>
		virtual void Update(float timeSinceLastFrame) override {
			if(!disposed) {
				internalPerlin->update(timeSinceLastFrame);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Save config.
		/// </summary>
		/// <param name="data">String reference.</param>
		virtual void SaveCfg(System::String^ data) override {
			if(!disposed) {
				internalPerlin->saveCfg(GetUnmanagedString(data));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Load config.
		/// </summary>
		/// <param name="cfgFile">A Mogre::ConfigFile.</param>
		/// <returns>True if it is the correct noise config.</returns>
		virtual bool LoadCfg(Mogre::ConfigFile^ cfgFile) override {
			if(!disposed) {
				return internalPerlin->loadCfg(cfgFile);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Get the specified x/y noise value.
		/// </summary>
		/// <param name="x">X Coord.</param>
		/// <param name="y">Y Coord.</param>
		/// <returns>Noise value.</returns>
		/// <remarks>range [~-0.2, ~0.2]</remarks>
		float GetValue(float x, float y) {
			if(!disposed) {
				return internalPerlin->getValue(x, y);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets the current perlin noise options.
		/// </summary>
		/// <returns>Current perlin noise options.</returns>
		/// <param name="value">Perlin noise options.</param>
		/// <remarks>If Create() has been already called, Octaves option can't be updated.</remarks>
		property MPerlin::MOptions^ Options {
			MPerlin::MOptions^ get() {
				if(!disposed) {
					return gcnew MPerlin::MOptions((Hydrax::Noise::Perlin::Options*)&internalPerlin->getOptions());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(MPerlin::MOptions^ value) {
				if(!disposed) {
					Hydrax::Noise::Perlin::Options opt = (Hydrax::Noise::Perlin::Options)*value;
					internalPerlin->setOptions(opt);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MPerlin() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources

				this->!MPerlin();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MPerlin() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalPerlin;
			}

			disposed = true;
		}

	};
}
