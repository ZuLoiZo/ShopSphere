import api from './api'

export interface Product {
  id: number
  name: string
  description: string
  price: number
  stock: number
  imageUrl: string
  categoryId: number
  categoryName: string
  isActive: boolean
}

export interface ProductListResponse {
  products: Product[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export const getProducts = async (params?: {
  page?: number
  pageSize?: number
  search?: string
  categoryId?: number
  minPrice?: number
  maxPrice?: number
}): Promise<ProductListResponse> => {
  const response = await api.get('/products', { params })
  return response.data
}

export const getProduct = async (id: number): Promise<Product> => {
  const response = await api.get(`/products/${id}`)
  return response.data
}

export const getProductsByCategory = async (categoryId: number): Promise<Product[]> => {
  const response = await api.get(`/products/category/${categoryId}`)
  return response.data
}
