\ wrapper to load Swig-generated libraries

\ Copyright (C) 2016,2017 Free Software Foundation, Inc.

\ This file is part of Gforth.

\ Gforth is free software; you can redistribute it and/or
\ modify it under the terms of the GNU General Public License
\ as published by the Free Software Foundation, either version 3
\ of the License, or (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program. If not, see http://www.gnu.org/licenses/.

cs-vocabulary gst \ needs to be case sensitive
get-current also gst definitions

c-library gstlib
    \c #define GST_USE_UNSTABLE_API
    \c #include <gst/gst.h>
    \c #include <gst/gl/gl.h>

    s" gstreamer-1.0" add-lib
    s" a a 0" vararg$ $!
    
    include gst.fs
end-c-library

set-current
