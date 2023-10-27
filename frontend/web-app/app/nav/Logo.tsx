'use client'

import { useParamsStore } from '@/hooks/useParamsStore'
import React from 'react'
import { AiOutlineCar } from 'react-icons/ai'

export default function Logo() {
    const reset = useParamsStore(state => state.reset);

    return (
        <div onClick={reset} className='flex items-center text-3xl gap-2 font-semibold text-red-500 cursor-pointer'>
            <AiOutlineCar size={34}></AiOutlineCar>
            <div>Carsties Auction</div>
        </div>
    )
}
