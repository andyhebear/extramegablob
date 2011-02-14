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

// MHelp.h

#pragma once

namespace MHydrax {

	/// <summary>
	/// Struct wich contains a specific width and height value.
	/// </summary>
	public value class MSize {
	private:
		int mWidth;
		int mHeight;

	internal:
		/// <summary>
		/// Creates a new instance of MSize.
		/// </summary>
		/// <param name="sz">A native Hydrax::Size.</param>
		MSize(Hydrax::Size sz) {
			mWidth = sz.Width;
			mHeight = sz.Height;
		}

	public:
		/// <summary>
		/// Conversion operator to type Hydrax::Size.
		/// </summary>
		operator Hydrax::Size() {
			Hydrax::Size sz;
			sz.Width = mWidth;
			sz.Height = mHeight;
			return sz;
		}

		virtual System::String^ ToString() override {
			return "(" + mHeight + ", " + mWidth + ")";
		}

		virtual bool Equals(System::Object^ obj) override {
			if(obj->GetType() == MHydrax::MSize::typeid) {
				return this->Equals((MSize)obj);
			}
			else {
				return false;
			}
		}

		bool Equals(MSize obj) {
			return ((mWidth == obj.Width) && (mHeight == obj.Height));
		}

		/// <summary>
		/// Creates a new instance of MSize.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		MSize(int width, int height) {
			mWidth = width;
			mHeight = height;
		}

		property int Width {
			int get() {
				return mWidth;
			}
			void set(int value) {
				mWidth = value;
			}
		}

		property int Height {
			int get() {
				return mHeight;
			}
			void set(int value) {
				mHeight = value;
			}
		}

	};

}
