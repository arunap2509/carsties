import { useParamsStore } from '@/hooks/useParamsStore';
import { Button } from 'flowbite-react';
import React from 'react'
import { AiOutlineClockCircle, AiOutlineSortAscending } from 'react-icons/ai';
import { BsFillStopCircleFill, BsStopwatchFill } from 'react-icons/bs';
import { GiFinishLine, GiFlame } from 'react-icons/gi';

const pageSizeButtons = [4, 8, 12];
const orderButton = [
    {
        label: 'Alphabetical',
        icon: AiOutlineSortAscending,
        value: 'make'
    },
    {
        label: 'End date',
        icon: AiOutlineClockCircle,
        value: 'endingSoon'
    },
    {
        label: 'Recently added',
        icon: BsFillStopCircleFill,
        value: 'new'
    }
]
const filterButton = [
    {
        label: 'Live Auctions',
        icon: GiFlame,
        value: 'make'
    },
    {
        label: 'Ending < 6 hours',
        icon: GiFinishLine,
        value: 'endingSoon'
    },
    {
        label: 'Completed',
        icon: BsStopwatchFill,
        value: 'finished'
    }
]

export default function Filter() {
    const pageSize = useParamsStore(state => state.pageSize);
    const orderBy = useParamsStore(state => state.orderBy);
    const filterBy = useParamsStore(state => state.filterBy);
    const setParams = useParamsStore(state => state.setParams);

    return (
        <div className='flex justify-between items-center mb-4'>
            <div>
                <span className='uppercase text-sm text-gray-500 mr-2'>Filter By</span>
                <Button.Group>
                    {filterButton.map(({ label, icon: Icon, value }) => (
                        <Button
                            key={value}
                            onClick={() => setParams({ filterBy: value })}
                            color={`${filterBy == value ? 'red' : 'gray'}`}
                        >
                            <Icon className='mr-3 h-4 w-4' />
                            {label}
                        </Button>
                    ))}
                </Button.Group>
            </div>
            <div>
                <span className='uppercase text-sm text-gray-500 mr-2'>Order By</span>
                <Button.Group>
                    {orderButton.map(({ label, icon: Icon, value }) => (
                        <Button
                            key={value}
                            onClick={() => setParams({ orderBy: value })}
                            color={`${orderBy == value ? 'red' : 'gray'}`}
                        >
                            <Icon className='mr-3 h-4 w-4' />
                            {label}
                        </Button>
                    ))}
                </Button.Group>
            </div>
            <div>
                <span className='uppercase text-sm text-gray-500 mr-2'>Page Size</span>
                <Button.Group>
                    {pageSizeButtons.map((val, i) => (
                        <Button
                            key={i}
                            onClick={() => setParams({ pageSize: val })}
                            color={`${pageSize == val ? 'red' : 'gray'}`}
                        >
                            {val}
                        </Button>
                    ))}
                </Button.Group>
            </div>
        </div>
    )
}
