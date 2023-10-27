'use client'

import { Pagination } from 'flowbite-react'
import React from 'react'

type Props = {
    currentPage: number
    totalCount: number,
    pageChanged: (page: number) => void
}

export default function AppPagination({ currentPage, totalCount, pageChanged }: Props) {

    return (
        <Pagination
            currentPage={currentPage}
            onPageChange={(e) => pageChanged(e)}
            totalPages={totalCount}
            layout='pagination'
            showIcons
            className='text-blue-500 mb-5'
        />
    )
}
