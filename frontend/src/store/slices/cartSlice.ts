import { createSlice, PayloadAction } from '@reduxjs/toolkit'

interface CartItem {
  id: number
  productId: number
  name: string
  price: number
  quantity: number
  imageUrl: string
}

interface CartState {
  items: CartItem[]
  total: number
  loading: boolean
}

const initialState: CartState = {
  items: [],
  total: 0,
  loading: false,
}

const cartSlice = createSlice({
  name: 'cart',
  initialState,
  reducers: {
    setCartItems: (state, action: PayloadAction<CartItem[]>) => {
      state.items = action.payload
      state.total = action.payload.reduce((sum, item) => sum + item.price * item.quantity, 0)
    },
    addToCart: (state, action: PayloadAction<CartItem>) => {
      const existingItem = state.items.find(item => item.productId === action.payload.productId)
      if (existingItem) {
        existingItem.quantity += action.payload.quantity
      } else {
        state.items.push(action.payload)
      }
      state.total = state.items.reduce((sum, item) => sum + item.price * item.quantity, 0)
    },
    removeFromCart: (state, action: PayloadAction<number>) => {
      state.items = state.items.filter(item => item.id !== action.payload)
      state.total = state.items.reduce((sum, item) => sum + item.price * item.quantity, 0)
    },
    updateQuantity: (state, action: PayloadAction<{ id: number; quantity: number }>) => {
      const item = state.items.find(item => item.id === action.payload.id)
      if (item) {
        item.quantity = action.payload.quantity
        state.total = state.items.reduce((sum, item) => sum + item.price * item.quantity, 0)
      }
    },
    clearCart: (state) => {
      state.items = []
      state.total = 0
    },
    setLoading: (state, action: PayloadAction<boolean>) => {
      state.loading = action.payload
    },
  },
})

export const { setCartItems, addToCart, removeFromCart, updateQuantity, clearCart, setLoading } = cartSlice.actions
export default cartSlice.reducer
