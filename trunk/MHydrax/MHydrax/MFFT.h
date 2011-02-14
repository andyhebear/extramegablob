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

// MFFT.h

#pragma once

#include "MNoise.h"

namespace MHydrax {

	/// <summary>
	/// FFT noise module class.
	/// </summary>
	public ref class MFFT : MNoise, System::IDisposable {
	public:
		/// <summary>
		/// Struct wich contains FFT noise module options.
		/// </summary>
		ref class MOptions {
		private:
			Hydrax::Noise::FFT::Options* internalOptions;

			// Track whether this instance owns, or does not own the reference to the native pointer.
			bool ownsNativeRef;

			// Track whether Dispose has been called.
			bool disposed;

		internal:
			/// <summary>
			/// Creates a new instance of MFFT.MOptions.
			/// </summary>
			/// <param name="nativeOptions">Native Noise::FFT::Options.</param>
			MOptions(Hydrax::Noise::FFT::Options* nativeOptions) {
				internalOptions = nativeOptions;
				ownsNativeRef = false;
			}

		public:
			/// <summary>
			/// Creates a new instance of MFFT.MOptions.
			/// </summary>
			MOptions() {
				internalOptions = new Hydrax::Noise::FFT::Options();
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MFFT.MOptions.
			/// </summary>
			/// <param name="resolution">FFT Resolution (2^n).</param>
			/// <param name="physicalResolution">Physical resolution of the surface.</param>
			/// <param name="scale">Noise scale.</param>
			/// <param name="windDirection">Wind direction.</param>
			/// <param name="animationSpeed">Animation speed coeficient.</param>
			/// <param name="kwPower">KwPower.</param>
			/// <param name="amplitude">Noise amplitude.</param>
			MOptions(int resolution,
					float physicalResolution,
					float scale,
					Mogre::Vector2 windDirection,
					float animationSpeed,
					float kwPower,
					float amplitude) {
				internalOptions = new Hydrax::Noise::FFT::Options(
					resolution, physicalResolution, scale, (Ogre::Vector2&)windDirection, animationSpeed, kwPower, amplitude);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MFFT.MOptions.
			/// </summary>
			/// <param name="resolution">FFT Resolution (2^n).</param>
			/// <param name="physicalResolution">Physical resolution of the surface.</param>
			/// <param name="scale">Noise scale.</param>
			/// <param name="windDirection">Wind direction.</param>
			/// <param name="animationSpeed">Animation speed coeficient.</param>
			/// <param name="kwPower">KwPower.</param>
			/// <param name="amplitude">Noise amplitude.</param>
			/// <param name="gpu_Strength">GPU_Strength.</param>
			/// <param name="gpu_LodParameters">GPU_LODParameters.</param>
			MOptions(int resolution,
					float physicalResolution,
					float scale,
					Mogre::Vector2 windDirection,
					float animationSpeed,
					float kwPower,
					float amplitude,
					float gpu_Strength,
					Mogre::Vector3 gpu_LodParameters) {
				internalOptions = new Hydrax::Noise::FFT::Options(
					resolution, physicalResolution, scale, (Ogre::Vector2&)windDirection, animationSpeed, kwPower, amplitude, gpu_Strength, (Ogre::Vector3&)gpu_LodParameters);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Conversion operator to type Hydrax::Noise::FFT::Options*.
			/// </summary>
			operator Hydrax::Noise::FFT::Options *() {
				if(!disposed) {
					return internalOptions;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Conversion operator to type Hydrax::Noise::FFT::Options.
			/// </summary>
			operator Hydrax::Noise::FFT::Options(){
				if(!disposed) {
					Hydrax::Noise::FFT::Options opt = Hydrax::Noise::FFT::Options(internalOptions->Resolution, internalOptions->PhysicalResolution, internalOptions->Scale, internalOptions->WindDirection, internalOptions->AnimationSpeed, internalOptions->KwPower, internalOptions->Amplitude, internalOptions->GPU_Strength, internalOptions->GPU_LODParameters);
					return opt;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Resolution.
			/// </summary>
			property int Resolution {
				int get() {
					if(!disposed) {
						return internalOptions->Resolution;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(int value) {
					if(!disposed) {
						internalOptions->Resolution = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Physical resolution.
			/// </summary>
			property int PhysicalResolution {
				int get() {
					if(!disposed) {
						return internalOptions->PhysicalResolution;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(int value) {
					if(!disposed) {
						internalOptions->PhysicalResolution = value;
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
			/// Wind direction.
			/// </summary>
			property Mogre::Vector2 WindDirection {
				Mogre::Vector2 get() {
					if(!disposed) {
						return GetManagedVector2(internalOptions->WindDirection);
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(Mogre::Vector2 value) {
					if(!disposed) {
						internalOptions->WindDirection = (Ogre::Vector2&)value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Animation speed.
			/// </summary>
			property float AnimationSpeed {
				float get() {
					if(!disposed) {
						return internalOptions->AnimationSpeed;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->AnimationSpeed = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// KwPower.
			/// </summary>
			property float KwPower {
				float get() {
					if(!disposed) {
						return internalOptions->KwPower;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->KwPower = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Amplitude.
			/// </summary>
			property float Amplitude {
				float get() {
					if(!disposed) {
						return internalOptions->Amplitude;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->Amplitude = value;
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
		Hydrax::Noise::FFT* internalFFT;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MFFT.
		/// </summary>
		///	<param name="nativeFFT">A native Hydrax::Noise::FFT.</param>
		MFFT(Hydrax::Noise::FFT* nativeFFT)
			: MNoise() {
			internalFFT = nativeFFT;
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
		/// Creates a new instance of MFFT.
		/// </summary>
		MFFT()
			: MNoise() {
			internalFFT = new Hydrax::Noise::FFT();
			ownsNativeRef = true;
		}

		/// <summary>
		/// Creates a new instance of MFFT.
		/// </summary>
		/// <param name="options">Options.</param>
		MFFT(MFFT::MOptions^ options)
			: MNoise() {
			Hydrax::Noise::FFT::Options nativeOptions = (Hydrax::Noise::FFT::Options)*options;
			internalFFT = new Hydrax::Noise::FFT(nativeOptions);
			ownsNativeRef = true;
		}

		/// <summary>
		/// Conversion operator to type Hydrax::Noise::Noise*.
		/// </summary>
		virtual operator Hydrax::Noise::Noise *() override {
			if(!disposed) {
				return (Hydrax::Noise::Noise*)internalFFT;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Conversion operator to type Hydrax::Noise::FFT*.
		/// </summary>
		operator Hydrax::Noise::FFT *() {
			if(!disposed) {
				return internalFFT;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Create.
		/// </summary>
		virtual void Create() override {
			if(!disposed) {
				internalFFT->create();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove.
		/// </summary>
		virtual void Remove() override {
			if(!disposed) {
				internalFFT->remove();
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
				internalFFT->update(timeSinceLastFrame);
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
				internalFFT->saveCfg(GetUnmanagedString(data));
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
				return internalFFT->loadCfg(cfgFile);
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
				return internalFFT->getValue(x, y);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets the current FFT noise options.
		/// </summary>
		/// <param name="value">FFT noise options.</param>
		/// <returns>Current FFT noise options.</returns>
		property MFFT::MOptions^ Options {
			MFFT::MOptions^ get() {
				if(!disposed) {
					return gcnew MFFT::MOptions((Hydrax::Noise::FFT::Options*)&internalFFT->getOptions());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(MFFT::MOptions^ value) {
				if(!disposed) {
					Hydrax::Noise::FFT::Options opt = (Hydrax::Noise::FFT::Options)*value;
					internalFFT->setOptions(opt);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MFFT() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources

				this->!MFFT();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MFFT() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalFFT;
			}

			disposed = true;
		}

	};
}
